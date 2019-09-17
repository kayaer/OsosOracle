using OsosOracle.Framework.Utilities.ExtensionMethods;
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
    public static class AutoCompleteHelper
    {


        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="htmlHelper"></param>
        /// <param name="expression"></param>
        /// <param name="ajaxFuction"></param>
        /// <param name="placeholder"></param>
        /// <param name="ekParametreler">genellikle gelen entity için yazılmış entityAra tipinde obje</param>
        /// <param name="tip">Liste veya tree</param>
        /// <param name="birimYetkiKontrol"></param>
        /// <param name="limit">gelecek kayıt sayısı</param>
        /// <param name="callback">seçim yapıldıktan sonra çalışacak js fonksiyon onselect(item : {value: "",text: ""}) </param>
        /// <param name="viewurl">tip customView seçildi ise mutlaka vir action adresi belirtilmelidir Örnek: /Hesap/index</param>
        /// <param name="viewUrlEkParametreler">Popup parametre göndermek için</param>
        /// <param name="disabled">seçim değiştirilebilir mi</param>
        /// <param name="showDescription"></param>
        /// <param name="clearCallback">temizleme yapılınca çağrılacak js func</param>
        /// <param name="multiple"></param>
        /// <param name="connectedWith">ilişkili olduğu diğer autoComplete <b>Html.idFor(exp) ile set edin!</b></param>
        /// <param name="connectedWith2">ilişkili olduğu diğer autoComplete <b>Html.idFor(exp) ile set edin!</b></param>
        /// <param name="connectedWithList">List</param>
        /// <param name="valueLabel"></param>
        /// <returns></returns>
        public static MvcHtmlString AutoComplete<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, TProperty>> expression,
            Enums.Enums.AutocompleteFunction ajaxFuction,
            Enums.Enums.AutoCompleteType tip = Enums.Enums.AutoCompleteType.List,
            string placeholder = "Aramak için yazın",
         object ekParametreler = null,
            bool birimYetkiKontrol = true,
            int limit = 10,
            string callback = "",
            string viewurl = "",
            object viewUrlEkParametreler = null,
            bool disabled = false,
            bool showDescription = false,
            string clearCallback = "",
            bool multiple = false,
            string connectedWith = "",
            string connectedWith2 = "",
            string connectedWithList = "",
            string valueLabel = ""
            )
        {



            string targetId = htmlHelper.IdFor(expression).ToString();

            var sb = new StringBuilder("<div class=\"input-group\" id=\"__").Append(targetId);
            sb.Append(disabled ? "\" style=\"width:100%\">\n" : "\" style=\"width:auto\">\n");


            var url = ajaxFuction.ToDescription();


            url += !url.Contains("?") ? $"?limit={limit}&" : $"&limit={limit}&";


            if (ekParametreler != null)
            {
                url += ekParametreler.ToQueryString();
            }

            if (viewUrlEkParametreler != null)
            {
                viewurl += !viewurl.Contains("?") ? "?" : "&";
                viewurl += viewUrlEkParametreler.ToQueryString();
            }

            /*TODO burası çözülecek
             * if (birimYetkiKontrol)
            {
                url += $"&birimid={YetkiAraci.AktifEkranYetkiliKapsamBirimID.ToInt32()}";
            }
            */



            //   string modelPropFullName = htmlHelper.NameFor(expression).ToString();

            //    var method = expression.Compile();

            // var value = method(htmlHelper.ViewData.Model);

            //List<string> lstModelPropName = expression.Body.ToString().Split('.').ToList();
            //lstModelPropName.RemoveAt(0);


            //lstModelPropName.ForEach(p => modelPropFullName = string.Format("{0}{1}.", modelPropFullName, p));
            //modelPropFullName = "_" + modelPropFullName.TrimEnd('.').Replace('.', '_');






            var properties = new RouteValueDictionary
            {
                {"placeholder", placeholder},
                {"id", $"_{targetId}"},
                {"title", placeholder},
                {"class", "form-control ac2 "},
                {"autocomplete", "off"},
                {"data-url", url},
                {"data-target", targetId},
                {"data-showDesc", showDescription},
                {"data-multiple", multiple},
                {"data-limit", limit},
                {"data-ajaxDeger", ((Enums.Enums.AutocompleteDegerFuction) ajaxFuction.GetHashCode()).ToDescription()},
                {"data-ajaxCokDeger", ((Enums.Enums.AutocompleteCokDegerFuction) ajaxFuction.GetHashCode()).ToDescription()}
            };


            if (!string.IsNullOrEmpty(callback))
            { properties.Add("data-callback", callback); }

            if (!string.IsNullOrEmpty(clearCallback))
            { properties.Add("data-clearcallback", clearCallback); }

            if (ekParametreler != null)
            { properties.Add("data-ekParametreler", ekParametreler.ToJson()); }

            if (!string.IsNullOrEmpty(viewurl))
            { properties.Add("data-viewurl", viewurl); }

            if (disabled)

            { properties.Add("disabled", "disabled"); }



            if (!string.IsNullOrEmpty(connectedWith))
            { properties.Add("data-connectedwith", connectedWith.Replace(".", "_")); }

            if (!string.IsNullOrEmpty(connectedWith2))
            { properties.Add("data-connectedwith2", connectedWith2.Replace(".", "_")); }

            if (!string.IsNullOrEmpty(connectedWithList))
            { properties.Add("data-connectedwithlist", connectedWithList.Replace(".", "_")); }



            sb.AppendLine(htmlHelper.TextBox($"_{targetId}", valueLabel, properties).ToString());

            sb.AppendLine(htmlHelper.HiddenFor(expression).ToString());

            if (!disabled) //disable yapılmışsa clear ve seçim gösterilmeyecek
            {
                sb.Append("<i class=\"fa fa-times text-danger autocomplete_clear\" id=\"ac_clear_").Append(targetId).Append("\" title=\"Temizle\" data-target=\"").Append(targetId)
                              .Append("\"></i>");

                if (tip != Enums.Enums.AutoCompleteType.None)
                    sb.Append("<span class=\"input-group-addon\"><a class=\"listopener\" title=\"").Append(placeholder).Append("\" data-target=\"").Append(targetId).Append("\" data-type=\"").Append(tip)
                                  .Append("\"><i class=\"fa fa-list-alt\"></i></a></span>");
            }
            sb.AppendLine("</div> ");
            if (multiple)
                sb.Append("<div class=\"ac_selection\" id=\"ac_selection_").Append(targetId).Append("\"></div>");

            return MvcHtmlString.Create(sb.ToString());

        }


    }
}
