using System;
using System.Collections;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Compilation;
using System.Web.UI;

namespace DevelopmentWithADot.AspNetExpressionBuilders
{
	[ExpressionPrefix("Format")]
	public sealed class FormatExpressionBuilder : ConvertedExpressionBuilder
	{
		#region Public static methods
		public static String Format(String values)
		{
			var parts = values.Split(',');
			var list = new ArrayList();
			var format = String.Empty;

			foreach (String part in parts)
			{
				var p = part.Trim();

				if (p.StartsWith("\'", StringComparison.OrdinalIgnoreCase) == true)
				{
					var i = p.IndexOf('\'', 1);

					if (String.IsNullOrWhiteSpace(format) == true)
					{
						format = p.Substring(1, i - 1);
					}
					else
					{
						list.Add(p.Substring(1, i - 1));
					}
				}
				else
				{
					Double d;
					Int64 l;
					DateTime dt;
					Decimal dc;

					if (DateTime.TryParse(p, out dt) == true)
					{
						list.Add(dt);
					}
					else if (Decimal.TryParse(p, out dc) == true)
					{
						list.Add(dc);
					}
					else if (Double.TryParse(p, out d) == true)
					{
						list.Add(d);
					}
					else if (Int64.TryParse(p, out l) == true)
					{
						list.Add(l);
					}
					else
					{
						if (p.Contains('.') == true)
						{
							var resourceParts = p.Split('.');
							var classKey = resourceParts[0];
							var resourceKey = resourceParts[1];
							var resourceValue = HttpContext.GetGlobalResourceObject(classKey, resourceKey, CultureInfo.CurrentUICulture).ToString();

							if (String.IsNullOrWhiteSpace(format) == true)
							{
								format = resourceValue;
							}
							else
							{
								list.Add(resourceValue);
							}

						}
						else
						{
							var resourceKey = p;
							var resourceValue = String.Empty;

							try
							{
								resourceValue = HttpContext.GetLocalResourceObject(HttpContext.Current.Request.Path, resourceKey, CultureInfo.CurrentUICulture).ToString();
							}
							catch (InvalidOperationException)
							{
							}

							if (resourceValue == null)
							{
								list.Add(p);
							}
							else
							{
								if (String.IsNullOrWhiteSpace(format) == true)
								{
									format = resourceValue;
								}
								else
								{
									list.Add(resourceValue);
								}
							}

						}
					}
				}
			}

			return (String.Format(CultureInfo.CurrentUICulture, format, list.ToArray()));
		}
		#endregion

		#region Public override methods

		/// <summary>
		/// When overridden in a derived class, returns an object that represents an evaluated expression.
		/// </summary>
		/// <param name="target">The object containing the expression.</param>
		/// <param name="entry">The object that represents information about the property bound to by the expression.</param>
		/// <param name="parsedData">The object containing parsed data as returned by <see cref="M:System.Web.Compilation.ExpressionBuilder.ParseExpression(System.String,System.Type,System.Web.Compilation.ExpressionBuilderContext)"/>.</param>
		/// <param name="context">Contextual information for the evaluation of the expression.</param>
		/// <returns>
		/// An object that represents the evaluated expression; otherwise, null if the inheritor does not implement <see cref="M:System.Web.Compilation.ExpressionBuilder.EvaluateExpression(System.Object,System.Web.UI.BoundPropertyEntry,System.Object,System.Web.Compilation.ExpressionBuilderContext)"/>.
		/// </returns>
		public override Object EvaluateExpression(Object target, BoundPropertyEntry entry, Object parsedData, ExpressionBuilderContext context)
		{
			return (Convert(Format(entry.Expression), entry.PropertyInfo.PropertyType));
		}

		#endregion

		#region Public override properties

		public override String MethodName
		{
			get { return("Format"); }
		}

		#endregion
	}
}
