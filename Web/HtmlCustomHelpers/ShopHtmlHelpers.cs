using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Web.HtmlCustomHelpers
{
    public static class ShopHtmlHelpers
    {
        public static MvcHtmlString ImgDeserialize(this HtmlHelper helper, byte[] source, string cssStyle)
        {
            if (source != null)
            {
                var base64 = Convert.ToBase64String(source);
                var imgSrc = string.Format("data:image;base64,{0}", base64);
                cssStyle = String.Format('"' + cssStyle + '"');
                return new MvcHtmlString(string.Format($"<img src={imgSrc} class={cssStyle}>"));
            }
            else return null;
        }
    }
}