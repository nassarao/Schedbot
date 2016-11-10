using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace SchedBot.Helpers
{
    public static class HtmlHelper
    {

        public static MvcHtmlString AlertError(ModelErrorCollection errors)
        {
            StringBuilder builder = new StringBuilder();
            if (errors.Count > 0)
            {
                builder.Append("<div class=\"alert alert-danger\"> <ul>");
                foreach (ModelError error in errors)
                {
                    builder.Append("<li>" + error.ErrorMessage + "</li>");
                }
                builder.Append("</ul></div>");
            }
            MvcHtmlString temp = new MvcHtmlString(builder.ToString());
            return temp;

        }
    }
}