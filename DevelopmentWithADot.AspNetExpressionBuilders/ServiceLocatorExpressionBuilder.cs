using System;
using System.Web.Compilation;
using System.Web.UI;
using Microsoft.Practices.ServiceLocation;

namespace DevelopmentWithADot.AspNetExpressionBuilders
{
	[ExpressionPrefix("ServiceLocator")]
	public sealed class ServiceLocatorExpressionBuilder : ConvertedExpressionBuilder
	{
		#region Public static methods

		public static Object GetInstance(String name, Type propertyType)
		{
			return (Convert(ServiceLocator.Current.GetInstance(propertyType, name), propertyType));
		}

		#endregion

		#region Public override methods

		public override Object EvaluateExpression(Object target, BoundPropertyEntry entry, Object parsedData, ExpressionBuilderContext context)
		{
			return (GetInstance(entry.Expression, entry.PropertyInfo.PropertyType));
		}

		#endregion

		#region Public override properties

		public override String MethodName
		{
			get { return("GetInstance"); }
		}

		#endregion
	}
}