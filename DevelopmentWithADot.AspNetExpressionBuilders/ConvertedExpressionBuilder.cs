using System;
using System.CodeDom;
using System.Web.Compilation;
using System.Web.UI;

namespace DevelopmentWithADot.AspNetExpressionBuilders
{
	public abstract class ConvertedExpressionBuilder : ExpressionBuilder
	{
		public abstract String MethodName { get; }

		protected static Object Convert(Object value, Type destinationType)
		{
			if (value == null)
			{
				return (null);
			}
  
			//if no destination type is supplied, just return the source object
			if (destinationType == null)
			{
				return (value);
			}

			//if types are compatible, return the source object
			//use IsAssignableFrom instead?
			if ((destinationType.IsInstanceOfType(value) == true) || (value.GetInterfaces().Contains(destinationType)))
			{
				return (value);
			}

			//if the destination type is string, just call ToString()
			if (destinationType == typeof(String))
			{
				return (value.ToString());
			}

			//if both types are IConvertible, call ChangeType()
			if ((typeof(IConvertible).IsAssignableFrom(destinationType) == true) && (typeof(IConvertible).IsAssignableFrom(value.GetType()) == true))
			{
				return (System.Convert.ChangeType(value, destinationType));
			}

			//if the destination type has a type converter that can handle the source object, use it
			var converter = TypeDescriptor.GetConverter(destinationType);

			if ((converter != null) && (converter.CanConvertFrom(value.GetType()) == true))
			{
				return (converter.ConvertFrom(value));
			}

			converter = TypeDescriptor.GetConverter(value.GetType());

			//if the source value has a type converter that can handle the destination type, use it
			if ((converter != null) && (converter.CanConvertTo(destinationType) == true))
			{
				return (converter.ConvertTo(value, destinationType));
			}

			//check if the type has an explicit conversion operator and use it
			var conversionOperator = value.GetType().GetMethod("op_Explicit", BindingFlags.Static | BindingFlags.Public);

			if ((conversionOperator.ReturnType == destinationType) && (conversionOperator.GetParameters().Length == 1) && (conversionOperator.GetParameters()[0].ParameterType.IsAssignableFrom(value.GetType()) == true))
			{
				return (conversionOperator.Invoke(null, new Object[] { value }));
			}

			//check if the type has an implicit conversion operator and use it
			conversionOperator = value.GetType().GetMethod("op_Implicit", BindingFlags.Static | BindingFlags.Public);

			if ((conversionOperator.ReturnType == destinationType) && (conversionOperator.GetParameters().Length == 1) && (conversionOperator.GetParameters()[0].ParameterType.IsAssignableFrom(value.GetType()) == true))
			{
				return (conversionOperator.Invoke(null, new Object[] { value }));
			}

			//maybe throw an exception instead?
			return (null);
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
