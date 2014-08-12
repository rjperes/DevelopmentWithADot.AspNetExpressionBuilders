using System;
using System.CodeDom;
using System.Web.Compilation;
using System.Web.UI;

namespace DevelopmentWithADot.AspNetExpressionBuilders
{
	public abstract class ConvertedExpressionBuilder : ExpressionBuilder
	{
		public abstract String MethodName { get; }

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

		/// <summary>
		/// When overridden in a derived class, returns a value indicating whether the current <see cref="T:System.Web.Compilation.ExpressionBuilder"/> object supports no-compile pages.
		/// </summary>
		/// <value></value>
		/// <returns>true if the <see cref="T:System.Web.Compilation.ExpressionBuilder"/> supports expression evaluation; otherwise, false.
		/// </returns>
		public override Boolean SupportsEvaluate
		{
			get
			{
				return (true);
			}
		}

		public sealed override CodeExpression GetCodeExpression(BoundPropertyEntry entry, Object parsedData, ExpressionBuilderContext context)
		{
			if (String.IsNullOrWhiteSpace(entry.Expression) == true)
			{
				return (new CodePrimitiveExpression(String.Empty));
			}
			else
			{
				return (new CodeMethodInvokeExpression(new CodeMethodReferenceExpression(new CodeTypeReferenceExpression(this.GetType()), this.MethodName), new CodePrimitiveExpression(entry.Expression), new CodeTypeOfExpression(entry.PropertyInfo.PropertyType)));
			}
		}
	}
}
