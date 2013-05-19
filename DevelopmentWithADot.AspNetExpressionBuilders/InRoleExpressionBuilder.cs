using System;
using System.CodeDom;
using System.Web;
using System.Web.Compilation;
using System.Web.UI;

namespace DevelopmentWithADot.AspNetExpressionBuilders
{
	[ExpressionPrefix("InRole")]
	public sealed class InRoleExpressionBuilder : ExpressionBuilder
	{
		#region Public override methods
		public override CodeExpression GetCodeExpression(BoundPropertyEntry entry, Object parsedData, ExpressionBuilderContext context)
		{
			if (String.IsNullOrWhiteSpace(entry.Expression) == true)
			{
				return (new CodePrimitiveExpression(String.Empty));
			}
			else
			{
				return (new CodeMethodInvokeExpression(new CodeMethodReferenceExpression(new CodeTypeReferenceExpression(this.GetType()), "InRole"), new CodePrimitiveExpression(entry.Expression)));
			}
		}

		public override Object EvaluateExpression(Object target, BoundPropertyEntry entry, Object parsedData, ExpressionBuilderContext context)
		{
			return (InRole(entry.Expression));
		}
		#endregion

		#region Public override properties
		public override Boolean SupportsEvaluate
		{
			get
			{
				return (true);
			}
		}
		#endregion

		#region Public static methods
		public static Boolean InRole(String role)
		{
			if (String.Equals(role, "*") == true)
			{
				return ((HttpContext.Current.User != null) && (HttpContext.Current.User.Identity != null) && (HttpContext.Current.User.Identity.IsAuthenticated == true));
			}
			else if (String.Equals(role, "?") == true)
			{
				return ((HttpContext.Current.User == null) || (HttpContext.Current.User.Identity == null) || (HttpContext.Current.User.Identity.IsAuthenticated == false));
			}
			else
			{
				String[] orRoles = role.Split(',', ' ', ';');
				Boolean matches = false;

				foreach (String orRole in orRoles)
				{
					String[] andRoles = orRole.Split('+');

					foreach (String andRole in andRoles)
					{
						if (HttpContext.Current.User.IsInRole(andRole) == false)
						{
							return (false);
						}
						else
						{
							matches = true;
						}
					}

					if (matches == true)
					{
						break;
					}
				}

				return (matches);
			}
		}
		#endregion
	}
}
