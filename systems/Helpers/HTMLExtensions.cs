using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Linq.Expressions;

namespace System.Web.Mvc.Html
{
    public static class HTMLExtensions
    {
        public static MvcHtmlString URLFor<TModel, TProperty>(this HtmlHelper<TModel> html, Expression<Func<TModel, TProperty>> expression, Object htmlAttributes)
        {
            MvcHtmlString URLfor = html.TextBoxFor(expression, htmlAttributes);
            return new MvcHtmlString(URLfor.ToHtmlString().Replace("type=\"text\"", "type=\"url\""));
        }
    }
}