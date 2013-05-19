using System;
using System.CodeDom;
using System.Reflection;
using System.Web;
using System.Web.Compilation;
using System.Web.UI;

namespace DevelopmentWithADot.AspNetExpressionBuilders
{
	[ExpressionPrefix("WebResourceUrl")]
	public sealed class WebResourceUrlExpressionBuilder : ExpressionBuilder
	{
		#region Public override methods
		public override CodeExpression GetCodeExpression(BoundPropertyEntry entry, Object parsedData, ExpressionBuilderContext context)
		{
			if (String.IsNullOrWhiteSpace(entry.Expression) == true)
			{
				return (new CodePrimitiveExpression(String.Empty));
			}
			else
			{
				String[] parts = entry.Expression.Split(',');

				if (parts.Length == 2)
				{
					return (new CodeMethodInvokeExpression(new CodeMethodReferenceExpression(new CodeTypeReferenceExpression(this.GetType()), "GetWebResourceUrl"), new CodePrimitiveExpression(parts[0]), new CodePrimitiveExpression(parts[1])));
				}
				else
				{
					return (new CodeMethodInvokeExpression(new CodeMethodReferenceExpression(new CodeTypeReferenceExpression(this.GetType()), "GetWebResourceUrl"), new CodePrimitiveExpression(entry.Expression)));
				}
			}
		}

		public override Object ParseExpression(String expression, Type propertyType, ExpressionBuilderContext context)
		{
			if (String.IsNullOrWhiteSpace(expression) == true)
			{
				return (base.ParseExpression(expression, propertyType, context));
			}
			else
			{
				String[] parts = expression.Split(',');

				if (parts.Length == 2)
				{
					return (GetWebResourceUrl(parts[0], parts[1]));
				}
				else
				{
					return (GetWebResourceUrl(expression));
				}
			}
		}
		#endregion

		#region Public override properties
		public override Boolean SupportsEvaluate
		{
			get
			{
				return (true);
			}
		}
		#endregion

		#region Public static methods
		public static String GetWebResourceUrl(String resourceName)
		{
			Page page = HttpContext.Current.Handler as Page;

			if (page != null)
			{
				Type type = page.GetType();

				if (type.Namespace == "ASP")
				{
					type = type.BaseType;
				}

				return (page.ClientScript.GetWebResourceUrl(type, resourceName));
			}

			return (String.Empty);
		}

		public static String GetWebResourceUrl(String assemblyName, String resourceName)
		{
			Page page = HttpContext.Current.Handler as Page;

			if (page != null)
			{
				Assembly asm = Assembly.Load(assemblyName);

				if (asm != null)
				{
					return (page.ClientScript.GetWebResourceUrl(asm.GetExportedTypes()[0], resourceName));
				}
			}

			return (String.Empty);
		}
		#endregion
	}
}