using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SolrCodeCamp.BaseballWeb.Models
{
    public static class HtmlHelpers
    {
        public static string ResolveUrl(this HtmlHelper helper, string path)
        {
            UrlHelper urlHelper = new UrlHelper(helper.ViewContext.RequestContext);

            return urlHelper.Content(path);
        }

        public static string TableHeaderLink(this HtmlHelper helper, BaseballView view, string idToSet, string headerToSet)
        {
            string linkFormat = "<a href=\"javascript:void(0);\" class=\"result_sort{0}\" id=\"id_{1}\">{2}</a>";

            string sortClass = (view.SortTerm == idToSet) ? " current_sort_header" : String.Empty;

            return string.Format(linkFormat, sortClass, idToSet, headerToSet);
        }
    }
}