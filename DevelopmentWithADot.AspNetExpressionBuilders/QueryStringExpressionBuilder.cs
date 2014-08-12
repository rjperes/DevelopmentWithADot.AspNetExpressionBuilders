using System;
using System.Web;
using System.Web.Compilation;
using System.Web.UI;

namespace DevelopmentWithADot.AspNetExpressionBuilders
{
    [ExpressionPrefix("QueryString")]
    public sealed class QueryStringExpressionBuilder : ConvertedExpressionBuilder
    {
        #region Public static methods

        public static Object GetQueryStringValue(String name, Type propertyType)
        {
            return (Convert(HttpContext.Current.Request.QueryString[name], propertyType));
        }

        #endregion

        #region Public override methods

        public override Object EvaluateExpression(Object target, BoundPropertyEntry entry, Object parsedData, ExpressionBuilderContext context)
        {
            return (GetQueryStringValue(entry.Expression, entry.PropertyInfo.PropertyType));
        }

        #endregion

        #region Public override properties

		public override String MethodName
		{
			get { return("GetQueryStringValue"); }
		}

        #endregion
    }
}