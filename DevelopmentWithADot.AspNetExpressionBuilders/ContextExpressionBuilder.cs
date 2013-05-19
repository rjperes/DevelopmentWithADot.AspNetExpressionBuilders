using System;
using System.CodeDom;
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

		public override CodeExpression GetCodeExpression(BoundPropertyEntry entry, Object parsedData, ExpressionBuilderContext context)
		{			
			if (String.IsNullOrWhiteSpace(entry.Expression) == true)
			{
				return (new CodePrimitiveExpression(String.Empty));
			}
			else
			{
				return (new CodeMethodInvokeExpression(new CodeMethodReferenceExpression(new CodeTypeReferenceExpression(this.GetType()), "GetValue"), new CodePrimitiveExpression(entry.Expression.Trim()), new CodeTypeOfExpression(entry.PropertyInfo.PropertyType)));
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
	}
}