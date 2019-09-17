using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace OsosOracle.Framework.HtmlHelpers
{
    public static class ButtonHelpers
    {


        public static MvcHtmlString ButtonLink(this HtmlHelper htmlHelper, string linkText, string actionName,
            string cssClass = "btn", string icon = "fa-check-circle",
            object routeValues = null, object htmlAttributes = null, string controller = "")
        {
            var lUrl = new UrlHelper(htmlHelper.ViewContext.RequestContext);

            var tb = new TagBuilder("a") { InnerHtml = string.Format("<i class='fa {0}'></i> {1}", icon, linkText) };
            tb.MergeAttribute("class", cssClass);
            tb.MergeAttribute("href",
                controller != ""
                    ? lUrl.Action(actionName, controller, routeValues)
                    : lUrl.Action(actionName, routeValues));

            if (htmlAttributes != null)
                tb.MergeAttributes(HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));

            return MvcHtmlString.Create(tb.ToString());
        }


        //    public static MvcHtmlString ButtonLink(this HtmlHelper htmlHelper, LocalizedString linkText, string actionName,
        //string cssClass = "btn", string icon = "fa-check-circle",
        //object routeValues = null, object htmlAttributes = null, string controller = "")
        //    {
        //        var lUrl = new UrlHelper(htmlHelper.ViewContext.RequestContext);

        //        var tb = new TagBuilder("a") { InnerHtml = string.Format("<i class='fa {0}'></i> {1}", icon, linkText.ToString()) };
        //        tb.MergeAttribute("class", cssClass);
        //        tb.MergeAttribute("href",
        //            controller != ""
        //                ? lUrl.Action(actionName, controller, routeValues)
        //                : lUrl.Action(actionName, routeValues));

        //        if (htmlAttributes != null)
        //            tb.MergeAttributes(HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));

        //        return MvcHtmlString.Create(tb.ToString());
        //    }


        public static MvcHtmlString BackButton(this HtmlHelper htmlHelper,
            string url = "",
            string linkText = "Geri",
            string cssClass = "btn btn-info btn-xs pull-right",
            string icon = "fa-reply",
            object htmlAttributes = null)
        {

            string returnUrl = htmlHelper.ViewContext.HttpContext.Request.QueryString.Get("returnurl");


            if (String.IsNullOrEmpty(url))
            {

                if (!String.IsNullOrEmpty(returnUrl))
                {
                    url = string.Format("/{0}", returnUrl);
                }
                else
                {
                    url = string.Format("/{0}", htmlHelper.ViewContext.RouteData.Values["controller"]);
                }

            }



            var tb = new TagBuilder("a") { InnerHtml = string.Format("<i class='fa {0}'></i> {1}", icon, linkText) };
            tb.MergeAttribute("class", cssClass);
            tb.MergeAttribute("href", url);

            if (htmlAttributes != null)
                tb.MergeAttributes(HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));

            return MvcHtmlString.Create(tb.ToString());
        }

        //public static MvcHtmlString CommentButton(this HtmlHelper htmlHelper,
        //    YorumKaynakTipi kaynakTip,
        //    long kaynakId,
        // string url = "/Yorum/index",
        // string linkText = "Yorumlar",
        // string cssClass = "modalizer btn btn-xs  pull-right",
        // string icon = "fa-comment",
        //    string title = "Yorumlar",
        // object htmlAttributes = null)
        //{

        //    var tb = new TagBuilder("a") { InnerHtml = string.Format("<i class='fa {0}'></i> {1}  <span data-kaynaktip=\"{2}\" data-kaynakid=\"{3}\" class=\"badge commentcount\">0</span>", icon, linkText, kaynakTip.GetHashCode(), kaynakId) };
        //    tb.MergeAttribute("class", cssClass);
        //    tb.MergeAttribute("title", title);
        //    tb.MergeAttribute("href", url + "?kaynakTip=" + kaynakTip.GetHashCode() + "&kaynakId=" + kaynakId);

        //    if (htmlAttributes != null)
        //        tb.MergeAttributes(HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));

        //    return MvcHtmlString.Create(tb.ToString());
        //}




        public static MvcHtmlString ReportExportButtonLink(this HtmlHelper htmlHelper, string linkText, string reportUrl, string extraCssClass)
        {
            var qsParam = HttpContext.Current.Request.QueryString.Count > 0 ? "&" : "?";

            string cssClass = extraCssClass + " btn dropdown-toggle";

            var sb = new StringBuilder();
            sb.Append("<div class='btn-group'>");
            sb.Append(string.Format(
                "<button type='button' class='{0}' data-toggle='dropdown' aria-haspopup='true' aria-expanded='false'>",
                cssClass));
            sb.Append(linkText);
            sb.Append(" <span class='caret'></span>").Append("</button>");
            sb.Append("<ul class='dropdown-menu'>");
            sb.Append(string.Format("<li><a href='{0}{1}exportType=pdf'>Pdf</a></li>", reportUrl, qsParam));
            sb.Append(string.Format("<li><a href='{0}{1}exportType=excel'>Excel</a></li>", reportUrl, qsParam));
            sb.Append(string.Format("<li><a href='{0}{1}exportType=word'>Word</a></li>", reportUrl, qsParam));
            sb.Append("</ul>").Append("</div>");

            return MvcHtmlString.Create(sb.ToString());
        }
    }
}
