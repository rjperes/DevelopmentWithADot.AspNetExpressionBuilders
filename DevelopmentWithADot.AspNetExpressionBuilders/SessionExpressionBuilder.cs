using System;
using System.CodeDom;
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

		public override CodeExpression GetCodeExpression(BoundPropertyEntry entry, Object parsedData, ExpressionBuilderContext context)
		{
			if (String.IsNullOrWhiteSpace(entry.Expression) == true)
			{
				return (new CodePrimitiveExpression(String.Empty));
			}
			else
			{
				return (new CodeMethodInvokeExpression(new CodeMethodReferenceExpression(new CodeTypeReferenceExpression(this.GetType()), "GetSessionValue"), new CodePrimitiveExpression(entry.Expression), new CodeTypeOfExpression(entry.PropertyInfo.PropertyType)));
			}
		}

		#endregion

		#region Public override properties

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
		#endregion
	}
}
