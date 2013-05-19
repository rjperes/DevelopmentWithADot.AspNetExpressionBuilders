using System;
using System.CodeDom;
using System.Web;
using System.Web.Compilation;
using System.Web.UI;

namespace DevelopmentWithADot.AspNetExpressionBuilders
{
	[ExpressionPrefix("ThemeFileUrl")]
	public sealed class ThemeFileUrlExpressionBuilder : ExpressionBuilder
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
				return (new CodeMethodInvokeExpression(new CodeMethodReferenceExpression(new CodeTypeReferenceExpression(this.GetType()), "GetThemeUrl"), new CodePrimitiveExpression(entry.Expression)));
			}
		}

		public override Object EvaluateExpression(Object target, BoundPropertyEntry entry, Object parsedData, ExpressionBuilderContext context)
		{
			if (String.IsNullOrWhiteSpace(entry.Expression) == true)
			{
				return base.EvaluateExpression(target, entry, parsedData, context);
			}
			else
			{
				return (GetThemeUrl(entry.Expression));
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
		public static String GetThemeUrl(String fileName)
		{
			Page page = HttpContext.Current.Handler as Page;
			String path = (page != null) ? String.Concat("/App_Themes/", page.Theme, "/", fileName) : String.Empty;

			return (path);
		}
		#endregion
	}
}