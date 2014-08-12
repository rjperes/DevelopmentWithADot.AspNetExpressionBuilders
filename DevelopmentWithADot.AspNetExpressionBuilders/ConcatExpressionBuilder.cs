using System;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Compilation;
using System.Web.UI;

namespace DevelopmentWithADot.AspNetExpressionBuilders
{
    [ExpressionPrefix("Concat")]
    public sealed class ConcatExpressionBuilder : ConvertedExpressionBuilder
    {
        #region Public static methods
        public static Object Concat(String values, Type propertyType)
        {
            StringBuilder builder = new StringBuilder();
            String[] parts = values.Split(',');

            foreach (String part in parts)
            {
                String p = part.Trim();

                if (p.StartsWith("\'", StringComparison.OrdinalIgnoreCase) == true)
                {
                    Int32 i = p.IndexOf('\'', 1);

                    builder.Append(p.Substring(1, i - 1));
                }
                else
                {
                    if (p.Contains('.') == true)
                    {
                        String[] resourceParts = p.Split('.');
                        String classKey = resourceParts[0];
                        String resourceKey = resourceParts[1];

                        builder.Append(HttpContext.GetGlobalResourceObject(classKey, resourceKey, CultureInfo.CurrentUICulture));
                    }
                    else
                    {
                        String resourceKey = p;

                        builder.Append(HttpContext.GetLocalResourceObject(HttpContext.Current.Request.Path, resourceKey, CultureInfo.CurrentUICulture));
                    }
                }
            }

            return (builder.ToString());
        }
        #endregion
       
        #region Public override methods

        public override Object EvaluateExpression(Object target, BoundPropertyEntry entry, Object parsedData, ExpressionBuilderContext context)
        {
            return (Concat(entry.Expression, entry.PropertyInfo.PropertyType));
        }

        #endregion

        #region Public override properties

		public override String MethodName
		{
			get { return("Concat"); }
		}

        #endregion        
    }
}
