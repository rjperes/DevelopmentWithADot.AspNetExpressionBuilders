using System;
using System.Web;
using System.Web.Compilation;
using System.Web.UI;

namespace DevelopmentWithADot.AspNetExpressionBuilders
{
	[ExpressionPrefix("InRole")]
	public sealed class InRoleExpressionBuilder : ConvertedExpressionBuilder
	{
		#region Public override methods
		public override Object EvaluateExpression(Object target, BoundPropertyEntry entry, Object parsedData, ExpressionBuilderContext context)
		{
			return (InRole(entry.Expression, entry.PropertyInfo.PropertyType));
		}
		#endregion

		#region Public override properties
		public override String MethodName
		{
			get { return("InRole"); }
		}
		#endregion

		#region Public static methods
		public static Boolean InRole(String role, Type propertyType)
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
