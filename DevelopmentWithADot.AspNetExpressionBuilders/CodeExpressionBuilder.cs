using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Reflection;
using System.Web;
using System.Web.Compilation;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.CSharp;

namespace DevelopmentWithADot.AspNetExpressionBuilders
{
	[ExpressionPrefix("Code")]
	public sealed class CodeExpressionBuilder : ExpressionBuilder
	{
		#region Public override methods
		public override CodeExpression GetCodeExpression(BoundPropertyEntry entry, Object parsedData, ExpressionBuilderContext context)
		{
			return (new CodeSnippetExpression(entry.Expression.Trim()));
		}

		public override Object EvaluateExpression(Object target, BoundPropertyEntry entry, Object parsedData, ExpressionBuilderContext context)
		{
			var item = HttpContext.Current.Handler as Page;
			var tempClass = new CodeTypeDeclaration("TempClass");
			tempClass.IsClass = true;

			var tempMethod = new CodeMemberMethod();
			tempMethod.Name = "GetValue";
			tempMethod.Attributes = MemberAttributes.Public;
			tempMethod.ReturnType = new CodeTypeReference(typeof(Object));
			tempMethod.Parameters.Add(new CodeParameterDeclarationExpression(typeof(String), "item"));

			if (item != null)
			{
				tempMethod.Parameters.Add(new CodeParameterDeclarationExpression(item.GetType(), "page"));
			}

			tempMethod.Statements.Add(new CodeMethodReturnStatement(this.GetCodeExpression(entry, parsedData, context)));
			tempClass.Members.Add(tempMethod);

			//Compile that class
			var unit = new CodeCompileUnit();
			var ns = new CodeNamespace("Temp");
			ns.Types.Add(tempClass);
			//Import declarations
			ns.Imports.Add(new CodeNamespaceImport("System"));
			ns.Imports.Add(new CodeNamespaceImport("System.Collections"));
			ns.Imports.Add(new CodeNamespaceImport("System.ComponentModel"));
			ns.Imports.Add(new CodeNamespaceImport("System.Data"));
			ns.Imports.Add(new CodeNamespaceImport("System.Reflection"));
			ns.Imports.Add(new CodeNamespaceImport("System.Web"));
			ns.Imports.Add(new CodeNamespaceImport("System.Web.UI"));
			ns.Imports.Add(new CodeNamespaceImport("System.Web.UI.WebControls"));
			ns.Imports.Add(new CodeNamespaceImport("System.Web.UI.HtmlControls"));

			unit.Namespaces.Add(ns);
			var compilerParams = new CompilerParameters();
			compilerParams.GenerateInMemory = true;

			//Add references
			var references = new List<String>();
			references.Add(typeof(String).Assembly.Location);
			references.Add(typeof(Component).Assembly.Location);
			references.Add(typeof(DataTable).Assembly.Location);
			references.Add(typeof(Button).Assembly.Location);
			references.Add(Assembly.GetCallingAssembly().Location);
			references.Add(Assembly.GetExecutingAssembly().Location);

			foreach (var name in references)
			{
				compilerParams.ReferencedAssemblies.Add(name);
			}

			var compiler = new CSharpCodeProvider();

			var results = compiler.CompileAssemblyFromDom(compilerParams, unit);

			var type = results.CompiledAssembly.GetExportedTypes()[0];

			var obj = Activator.CreateInstance(type);

			var mi = obj.GetType().GetMethod("GetValue", BindingFlags.Instance | BindingFlags.Public | BindingFlags.InvokeMethod);

			if (item != null)
			{
				return (Convert(mi.Invoke(obj, new Object [] { item, entry.Expression }), entry.PropertyInfo.PropertyType));
			}
			else
			{
				return (Convert(mi.Invoke(obj, new Object[] { entry.Expression }), entry.PropertyInfo.PropertyType));
			}
		}
		#endregion

		#region Protected static methods
		protected static Object Convert(Object value, Type type)
		{
			if (value == null)
			{
				return (value);
			}
			else if (type == typeof(String))
			{
				return (value.ToString());
			}
			else if ((value is IConvertible) && (typeof(IConvertible).IsAssignableFrom(type) == true))
			{
				return (System.Convert.ChangeType(value, type));
			}
			else if (type.IsEnum == true)
			{
				return (Enum.Parse(type, value.ToString(), true));
			}
			else
			{
				return (value);
			}
		}
		#endregion
	}
}