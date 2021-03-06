using System;
using System.Web;
using System.Web.Compilation;
using System.Web.UI;

namespace DevelopmentWithADot.AspNetExpressionBuilders
{
	[ExpressionPrefix("Profile")]
	public sealed class ProfileExpressionBuilder : ConvertedExpressionBuilder
	{
		#region Public override methods

		public override Object EvaluateExpression(Object target, BoundPropertyEntry entry, Object parsedData, ExpressionBuilderContext context)
		{
			return (GetProfileProperty(entry.Expression, entry.PropertyInfo.PropertyType));
		}
		#endregion

		#region Public override properties
		public override String MethodName
		{
			get { return("GetProfileProperty"); }
		}

		#endregion

		#region Public static methods
		public static Object GetProfileProperty(String propertyName, Type propertyType)
		{
			Object value = null;

			if (propertyName.Contains(".") == false)
			{
				value = HttpContext.Current.Profile.GetPropertyValue(propertyName);
			}
			else
			{
				value = DataBinder.Eval(HttpContext.Current.Profile, propertyName);
			}

			return (Convert(value, propertyType));
		}
		#endregion
	}
}
