using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SchedBot.Helpers
{
    public static class Helper
    {
        public static string TimeSpanToString(TimeSpan span)
        {
            string formated = "";
            string mins = "";
            string hours = "";
            if (span.Minutes < 10)
            {
                mins = "0" + span.Minutes;
            }
            else
                mins = span.Minutes.ToString();
            if (span.Hours < 12)
            {
                formated = String.Format("{0}:{1} AM", span.Hours, mins);
            }
            else
            {
                hours = (span.Hours - 12).ToString();
                if (span.Hours == 12)
                    hours = "12";
                formated = String.Format("{0}:{1} PM", hours, mins);
            }
            return formated;
        }
    }
}