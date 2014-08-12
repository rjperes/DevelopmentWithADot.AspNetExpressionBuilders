using System;
using System.Web;
using System.Web.Compilation;
using System.Web.UI;

namespace DevelopmentWithADot.AspNetExpressionBuilders
{
	[ExpressionPrefix("WebResourceUrl")]
	public sealed class WebResourceUrlExpressionBuilder : ConvertedExpressionBuilder
	{
		#region Public override methods

		public override Object ParseExpression(String expression, Type propertyType, ExpressionBuilderContext context)
		{
			if (String.IsNullOrWhiteSpace(expression) == true)
			{
				return (base.ParseExpression(expression, propertyType, context));
			}
			else
			{
				return (Convert(GetWebResourceUrl(expression), propertyType));
			}
		}
		#endregion

		#region Public override properties
		public override String MethodName
		{
			get { return("GetWebResourceUrl"); }
		}
		#endregion

		#region Public static methods
		public static String GetWebResourceUrl(String resourceName)
		{
			var page = HttpContext.Current.Handler as Page;

			if (page != null)
			{
				var type = page.GetType();

				if (type.Namespace == "ASP")
				{
					type = type.BaseType;
				}

				return (page.ClientScript.GetWebResourceUrl(type, resourceName));
			}

			return (String.Empty);
		}
		#endregion
	}
}