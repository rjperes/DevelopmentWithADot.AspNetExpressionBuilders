using System;
using System.Web.Compilation;

namespace DevelopmentWithADot.AspNetExpressionBuilders
{
	public abstract class ConvertedExpressionBuilder : ExpressionBuilder
	{
		protected static Object Convert(Object value, Type type)
		{
			if (value == null)
			{
				return (value);
			}
			else if (type == typeof(String))
			{
				return (value.ToString());
			}
			else if ((value is IConvertible) && (typeof(IConvertible).IsAssignableFrom(type) == true))
			{
				return (System.Convert.ChangeType(value, type));
			}
			else if (type.IsEnum == true)
			{
				return (Enum.Parse(type, value.ToString(), true));
			}
			else
			{
				return (value);
			}
		}
	}
}
