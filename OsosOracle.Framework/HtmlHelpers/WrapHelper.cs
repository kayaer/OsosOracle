using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace OsosOracle.Framework.HtmlHelpers
{
    public static class WrapHelper
    {
        public static IDisposable BeginWrap(this HtmlHelper helper, string tag, object htmlAttributes)
        {
            var builder = new TagBuilder(tag);
            var attrs = GetAttributes(htmlAttributes);
            if (attrs != null)
            {
                builder.MergeAttributes<string, object>(attrs);
            }
            helper.ViewContext.Writer.Write(builder.ToString(TagRenderMode.StartTag));

            return new WrapSection(helper, builder);
        }

        private static IDictionary<string, object> GetAttributes(object htmlAttributes)
        {
            if (htmlAttributes == null)
            {
                return null;
            }
            var dict = htmlAttributes as IDictionary<string, object>;
            if (dict != null)
            {
                return dict;
            }
            return HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);
        }

        private class WrapSection : IDisposable
        {
            private readonly HtmlHelper _helper;
            private readonly TagBuilder _tag;

            public WrapSection(HtmlHelper helper, TagBuilder tag)
            {
                _helper = helper;
                _tag = tag;
            }

            public void Dispose()
            {
                _helper.ViewContext.Writer.Write(_tag.ToString(TagRenderMode.EndTag));
            }
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="title"></param>
        /// <param name="subtitle"></param>
        /// <param name="icon"></param>
        /// <param name="htmlAttributes"></param>
        /// <param name="fullscreen"></param>
        /// <param name="tip"> box light solid  ayrıca renk yazılabilir</param>
        /// <returns></returns>
        public static IDisposable BeginPortlet(this HtmlHelper helper, string title, string subtitle = "", string icon = "", object htmlAttributes = null, bool fullscreen = false, string tip = "light")
        {


            helper.ViewContext.Writer.Write($"<div class=\"portlet {tip} bordered\"><div class=\"portlet-title\"><div class=\"caption\"><i class=\"{icon}\"></i>");


            helper.ViewContext.Writer.Write($"<span class=\"caption-subject bold \">{title}</span>");

            helper.ViewContext.Writer.Write($" <span class=\"caption-helper\"> {subtitle}</span>");

            helper.ViewContext.Writer.Write("</div><div class=\"tools\">");
            helper.ViewContext.Writer.Write("<a href = \"#\" class=\"collapse\" data-original-title=\"Aç/Kapa\" title=\"Aç/Kapa\"> </a>");
            if (fullscreen) helper.ViewContext.Writer.Write("<a class=\"fullscreen\" href=\"#\" data-original-title=\"Tam Ekran\" title=\"Tam Ekran\"> </a>");
            helper.ViewContext.Writer.Write("</div></div><div class=\"portlet-body\">");


            return new ClosePortlet(helper);
        }

        private class ClosePortlet : IDisposable
        {
            private readonly HtmlHelper _helper;


            public ClosePortlet(HtmlHelper helper)
            {
                _helper = helper;

            }

            public void Dispose()
            {
                _helper.ViewContext.Writer.Write("</div></div>");
            }
        }
    }
}
