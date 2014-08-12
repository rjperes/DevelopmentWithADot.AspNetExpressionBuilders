using System;
using System.Web;
using System.Web.Compilation;
using System.Web.UI;

namespace DevelopmentWithADot.AspNetExpressionBuilders
{
	[ExpressionPrefix("Session")]
	public sealed class SessionExpressionBuilder : ConvertedExpressionBuilder
	{
		#region Public static methods

		public static Object GetSessionValue(String name, Type propertyType)
		{
			Object value = null;

			if (name.Contains(".") == false)
			{
				value = HttpContext.Current.Session [ name ];
			}
			else
			{
				String [] parts = name.Split('.');				

				value = DataBinder.Eval(HttpContext.Current.Session[parts[0]], String.Join(".", parts, 1, parts.Length - 1));
			}

			return (Convert(value, propertyType));
		}

		#endregion

		#region Public override methods

		public override Object EvaluateExpression(Object target, BoundPropertyEntry entry, Object parsedData, ExpressionBuilderContext context)
		{
			return (GetSessionValue(entry.Expression, entry.PropertyInfo.PropertyType));
		}

		#endregion

		#region Public override properties

		public override String MethodName
		{
			get { return ("GetSessionValue"); }
		}

		#endregion
	}
}
