using System;
using System.Web;
using System.Web.Compilation;
using System.Web.UI;

namespace DevelopmentWithADot.AspNetExpressionBuilders
{
	[ExpressionPrefix("ThemeFileUrl")]
	public sealed class ThemeFileUrlExpressionBuilder : ConvertedExpressionBuilder
	{
		#region Public override methods
		public override Object EvaluateExpression(Object target, BoundPropertyEntry entry, Object parsedData, ExpressionBuilderContext context)
		{
			if (String.IsNullOrWhiteSpace(entry.Expression) == true)
			{
				return (base.EvaluateExpression(target, entry, parsedData, context));
			}
			else
			{
				return (Convert(GetThemeUrl(entry.Expression), entry.PropertyInfo.PropertyType));
			}
		}
		#endregion

		#region Public override properties

		public override String MethodName
		{
			get { return ("GetThemeUrl"); }
		}
		#endregion

		#region Public static methods
		public static String GetThemeUrl(String fileName)
		{
			var page = HttpContext.Current.Handler as Page;
			var path = (page != null) ? String.Concat("/App_Themes/", page.Theme, "/", fileName) : String.Empty;

			return (path);
		}
		#endregion
	}
}