using SchedbotDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SchedBot.Areas.Management.Models
{
    public class CalanderScheduleViewModel
    {

        public List<Event> events { get; set; }
    }

    public class Event
    {

        public string title { get; set; }
        public string start { get; set; }
        public string end { get; set; }
        public string backgroundColor { get; set; }
        public bool allDay { get; set; }
        public string borderColor { get; set; }
        public bool editable { get; set; }

        public Event(User_Shift_Schedule uss)
        {


            foreach (DateTime day in EachDay(uss.Schedule.StartDate, uss.Schedule.EndDate))
            {
                if (uss.Shift.Day == day.DayOfWeek)
                {
                    start = day.Add(uss.Shift.Start).ToString("s");
                    end = day.Add(uss.Shift.End).ToString("s");

                }
            }

            title = String.Format("{0} {1} - {2}", uss.User.FirstName, uss.User.LastName, uss.Shift.JobRole.Name);

            backgroundColor = "red";
            borderColor = "white";
            allDay = false;
            editable = false;


        }
        private IEnumerable<DateTime> EachDay(DateTime from, DateTime thru)
        {
            for (var day = from.Date; day.Date <= thru.Date; day = day.AddDays(1))
                yield return day;
        }

    }
}