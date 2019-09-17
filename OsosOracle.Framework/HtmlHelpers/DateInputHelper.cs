using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;

namespace OsosOracle.Framework.HtmlHelpers
{
    public static class DateInputHelper
    {
        public static MvcHtmlString DateInput<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, TProperty>> expression, object htmlAttributes = null,
             string placeholder = "Tarih seçiniz",
            DateTime? minDateTime = null,
            DateTime? maxDateTime = null,
            string format = "dd.MM.yyyy",
            string formatJs = "dd.mm.yyyy"

            )
        {
            RouteValueDictionary attrs = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);

            if (!attrs.ContainsKey("class"))
            {
                attrs.Add("class", "form-control datePicker");
            }

            if (!attrs.ContainsKey("placeholder"))
            {
                attrs.Add("placeholder", placeholder);
            }

            if (minDateTime.HasValue)
            {
                attrs.Add("data-date-startDate", minDateTime.Value.ToString(format));
            }


            if (maxDateTime.HasValue)
            {
                attrs.Add("data-date-endDate", maxDateTime.Value.ToString(format));
            }

            attrs.Add("data-date-format", formatJs);

            var sb = new StringBuilder("<div class=\"input-group \">\n");
            sb.AppendLine("<div class=\"input-group-addon\"><i class=\"fa fa-calendar\"></i></div>");
            sb.AppendLine(htmlHelper.TextBoxFor(expression, $"{{0:{format}}}", attrs).ToString());
            sb.AppendLine("</div> ");
            return MvcHtmlString.Create(sb.ToString());
        }

        //public static MvcHtmlString DateFor<TModel, TValue>(this HtmlHelper<TModel> html, 
        //    Expression<Func<TModel, TValue>> expression,
        //    object htmlAttributes = null,
        //    string placeholder="Tarih seçiniz",
        //    DateTime? minDateTime=null,
        //    DateTime? maxDateTime=null,
        //    string format = "dd.MM.yyyy",
        //    string formatJs = "dd.mm.yyyy")
        //{

        //    RouteValueDictionary attrs = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);

        //    if (!attrs.ContainsKey("class"))
        //    {
        //        attrs.Add("class", "form-control datePicker");
        //    }
        //    else
        //    {
        //        attrs["class"]= "form-control datePicker";
        //    }

        //    if (!attrs.ContainsKey("placeholder"))
        //    {
        //        attrs.Add("placeholder", placeholder);
        //    }

        //    if (minDateTime.HasValue)
        //    {
        //        attrs.Add("data-date-startDate",minDateTime.Value.ToString(format));
        //    }


        //    if (maxDateTime.HasValue)
        //    {
        //        attrs.Add("data-date-endDate", maxDateTime.Value.ToString(format));
        //    }

        //    attrs.Add("data-date-format", formatJs);

        //    var compilationResult = expression.Compile();
        //    TValue dateValue = compilationResult((TModel)html.ViewDataContainer.ViewData.Model);
        //    var body = (MemberExpression)expression.Body;
        //    attrs.Add("id", body.Member.Name);

        //    return html.TextBox(body.Member.Name, (Convert.ToDateTime(dateValue)).ToString(format),attrs);
        //}


        public static MvcHtmlString DateTimeInput<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, TProperty>> expression, object htmlAttributes = null)
        {
            RouteValueDictionary attrs = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);

            //if (!attrs.ContainsKey("class"))
            //{
            attrs.Add("class", "form-control dateTimePicker");
            //}


            if (!attrs.ContainsKey("placeholder"))
            {
                attrs.Add("placeholder", "Tarih Saat Seçiniz");
            }

            var sb = new StringBuilder("<div class=\"input-group \">\n");
            sb.AppendLine("<div class=\"input-group-addon\"><i class=\"fa fa-clock-o\"></i></div>");
            sb.AppendLine(htmlHelper.TextBoxFor(expression, null, attrs).ToString());
            sb.AppendLine("</div> ");
            return MvcHtmlString.Create(sb.ToString());
        }



    }
}
