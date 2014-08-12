using System;
using System.Web;
using System.Web.Compilation;
using System.Web.UI;

namespace DevelopmentWithADot.AspNetExpressionBuilders
{
	[ExpressionPrefix("Application")]
	public sealed class ApplicationExpressionBuilder : ConvertedExpressionBuilder
	{
		#region Public override methods
		public override Object EvaluateExpression(Object target, BoundPropertyEntry entry, Object parsedData, ExpressionBuilderContext context)
		{
			return (GetApplicationValue(entry.Expression, entry.PropertyInfo.PropertyType));
		}
		#endregion

		#region Public override properties

		public override String MethodName
		{
			get { return ("GetApplicationValue"); }
		}
		#endregion

		#region Public static methods
		public static Object GetApplicationValue(String name, Type propertyType)
		{
			Object value = null;

			if (name.Contains(".") == false)
			{
				value = HttpContext.Current.Application [ name ];
			}
			else
			{
				var parts = name.Split('.');

				value = DataBinder.Eval(HttpContext.Current.Application [ parts [ 0 ] ], String.Join(".", parts, 1, parts.Length - 1));
			}

			return (Convert(value, propertyType));
		}
		#endregion
	}
}
