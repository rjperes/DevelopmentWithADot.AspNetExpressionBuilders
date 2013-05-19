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
		#region Public override properties
		public override Boolean SupportsEvaluate
		{
			get
			{
				return (true);
			}
		}
		#endregion

		#region Public override methods
		public override CodeExpression GetCodeExpression(BoundPropertyEntry entry, Object parsedData, ExpressionBuilderContext context)
		{
			return (new CodeSnippetExpression(entry.Expression.Trim()));
		}

		public override Object EvaluateExpression(Object target, BoundPropertyEntry entry, Object parsedData, ExpressionBuilderContext context)
		{
			Page item = HttpContext.Current.Handler as Page;
			CodeTypeDeclaration tempClass = new CodeTypeDeclaration("TempClass");
			tempClass.IsClass = true;

			CodeMemberMethod tempMethod = new CodeMemberMethod();
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
			CodeCompileUnit unit = new CodeCompileUnit();
			CodeNamespace ns = new CodeNamespace("Temp");
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
			CompilerParameters compilerParams = new CompilerParameters();
			compilerParams.GenerateInMemory = true;
			//Add references
			List<String> references = new List<String>();

			references.Add(typeof(String).Assembly.Location);
			references.Add(typeof(Component).Assembly.Location);
			references.Add(typeof(DataTable).Assembly.Location);
			references.Add(typeof(Button).Assembly.Location);
			references.Add(Assembly.GetCallingAssembly().Location);
			references.Add(Assembly.GetExecutingAssembly().Location);
			
			foreach (String name in references)
			{
				compilerParams.ReferencedAssemblies.Add(name);
			}

			CodeDomProvider compiler = new CSharpCodeProvider();

			CompilerResults results = compiler.CompileAssemblyFromDom(compilerParams, unit);

			Type type = results.CompiledAssembly.GetExportedTypes()[0];

			Object obj = Activator.CreateInstance(type);

			MethodInfo mi = obj.GetType().GetMethod("GetValue", BindingFlags.Instance | BindingFlags.Public | BindingFlags.InvokeMethod);

			if (item != null)
			{
				return (mi.Invoke(obj, new Object [] { item, entry.Expression }));
			}
			else
			{
				return (mi.Invoke(obj, new Object [] { entry.Expression }));
			}
		}
		#endregion
	}
}