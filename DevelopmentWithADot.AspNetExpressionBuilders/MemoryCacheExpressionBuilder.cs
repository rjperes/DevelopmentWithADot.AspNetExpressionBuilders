using System;
using System.Runtime.Caching;
using System.Web;
using System.Web.Compilation;
using System.Web.UI;

namespace DevelopmentWithADot.AspNetExpressionBuilders
{
	[ExpressionPrefix("MemoryCache")]
	public sealed class MemoryCacheExpressionBuilder : ConvertedExpressionBuilder
	{
		#region Public override methods
		public override Object EvaluateExpression(Object target, BoundPropertyEntry entry, Object parsedData, ExpressionBuilderContext context)
		{
			return (GetCacheValue(entry.Expression, entry.PropertyInfo.PropertyType));
		}
		#endregion

		#region Public override properties

		public override String MethodName
		{
			get { return ("GetCacheValue"); }
		}
		#endregion

		#region Public static methods
		public static Object GetCacheValue(String name, Type propertyType)
		{
			Object value = null;

			if (name.Contains(".") == false)
			{
				value = MemoryCache.Default [ name ];
			}
			else
			{
				var parts = name.Split('.');

				value = DataBinder.Eval(MemoryCache.Default [parts[0]], String.Join(".", parts, 1, parts.Length - 1));
			}

			return (Convert(value, propertyType));
		}
		#endregion
	}
}
