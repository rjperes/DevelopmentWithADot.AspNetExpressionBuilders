using System;
using System.CodeDom;
using System.Reflection;
using System.Web;
using System.Web.Compilation;
using System.Web.UI;

namespace DevelopmentWithADot.AspNetExpressionBuilders
{
	[ExpressionPrefix("ViewState")]
	public sealed class ViewStateExpressionBuilder : ConvertedExpressionBuilder
	{
		#region Public static methods

		public static Object GetViewStateValue(String name, Type propertyType)
		{
			String [] parts = name.Split('.');
			Page page = HttpContext.Current.Handler as Page;
			Control control = page;
			String propertyName = (parts.Length == 2) ? parts [ 1 ] : name;

			if (parts.Length == 2)
			{
				control = page.FindControl(parts [ 0 ]);
			}

			if (control == null)
			{
				return (null);
			}

			PropertyInfo pi = control.GetType().GetProperty("ViewState", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.GetProperty);
			StateBag viewState = pi.GetValue(control, null) as StateBag;

			return (Convert(viewState [ propertyName ], propertyType));
		}

		#endregion

		#region Public override methods

		public override Object EvaluateExpression(Object target, BoundPropertyEntry entry, Object parsedData, ExpressionBuilderContext context)
		{
			return (GetViewStateValue(entry.Expression, entry.PropertyInfo.PropertyType));
		}

		public override CodeExpression GetCodeExpression(BoundPropertyEntry entry, Object parsedData, ExpressionBuilderContext context)
		{
			if (String.IsNullOrWhiteSpace(entry.Expression) == true)
			{
				return (new CodePrimitiveExpression(String.Empty));
			}
			else
			{
				return (new CodeMethodInvokeExpression(new CodeMethodReferenceExpression(new CodeTypeReferenceExpression(this.GetType()), "GetViewStateValue"), new CodePrimitiveExpression(entry.Expression), new CodeTypeOfExpression(entry.PropertyInfo.PropertyType)));
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
