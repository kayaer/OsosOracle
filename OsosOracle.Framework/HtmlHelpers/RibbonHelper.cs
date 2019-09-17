using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace OsosOracle.Framework.HtmlHelpers
{
    public static class RibbonHelper
    {
        public static IDisposable BeginRibbon(this HtmlHelper helper, string title, string ccsClass = "", RibbonColor ribbonColor = RibbonColor.Default)
        {
            helper.ViewContext.Writer.Write($"<div class=\"portlet mt-element-ribbon light portlet-fit bordered {ccsClass}\" style=\"border: 1px solid #bfbfbf !important;\">");
            helper.ViewContext.Writer.Write(
                $"<div class=\"ribbon ribbon-border-hor ribbon-clip ribbon-color-{ribbonColor.ToString().ToLowerInvariant()} \"><div class=\"ribbon-sub ribbon-clip\"></div> {title} </div>");

            helper.ViewContext.Writer.Write("<div class=\"ribbon-content\">");

            return new CloseRibbon(helper);
        }

        public enum RibbonColor
        {
            Default,
            Primary,
            Info,
            Success,
            Danger,
            Warning
        }

        private class CloseRibbon : IDisposable
        {
            private readonly HtmlHelper _helper;

            public CloseRibbon(HtmlHelper helper)
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
