using System;
using System.Web;
using System.Web.Compilation;
using System.Web.UI;

namespace DevelopmentWithADot.AspNetExpressionBuilders
{
	[ExpressionPrefix("Context")]
	public sealed class ContextExpressionBuilder : ConvertedExpressionBuilder
	{
		#region Public static methods
		public static Object GetValue(String expression, Type propertyType)
		{
			HttpContext context = HttpContext.Current;
			Object expressionValue = DataBinder.Eval(context, expression.Trim().Replace('\'', '"'));

			return (Convert(expressionValue, propertyType));
		}
		#endregion

		#region Public override methods
		public override Object EvaluateExpression(Object target, BoundPropertyEntry entry, Object parsedData, ExpressionBuilderContext context)
		{
			return (GetValue(entry.Expression, entry.PropertyInfo.PropertyType));
		}

		#endregion

		#region Public override properties

		public override String MethodName
		{
			get { return("GetValue"); }
		}
		#endregion
	}
}