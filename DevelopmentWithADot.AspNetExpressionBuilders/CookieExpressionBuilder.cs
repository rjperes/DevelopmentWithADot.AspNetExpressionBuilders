using System;
using System.Web;
using System.Web.Compilation;
using System.Web.UI;

namespace DevelopmentWithADot.AspNetExpressionBuilders
{
	[ExpressionPrefix("Cookie")]
	public sealed class CookieExpressionBuilder : ConvertedExpressionBuilder
	{
		#region Public override methods
		public override Object EvaluateExpression(Object target, BoundPropertyEntry entry, Object parsedData, ExpressionBuilderContext context)
		{
			return (GetCookieValue(entry.Expression, entry.PropertyInfo.PropertyType));
		}
		#endregion

		#region Public override properties

		public override String MethodName
		{
			get { return ("GetCookieValue"); }
		}

		#endregion

		#region Public static methods
		public static Object GetCookieValue(String cookieKey, Type propertyType)
		{
			String value = null;

			if (cookieKey.Contains(".") == true)
			{
				String [] cookieParts = cookieKey.Split('.');
				value = HttpContext.Current.Request.Cookies[cookieParts[0]].Values[cookieParts[1]];
			}
			else
			{
				value = HttpContext.Current.Request.Cookies [ cookieKey ].Value;
			}

			return (Convert(value, propertyType));
		}
		#endregion
	}
}
