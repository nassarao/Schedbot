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

    public class Event{

        public string title { get; set; }
        public DateTime start { get; set; }
        public DateTime end { get; set; }
        public string backgroundColor { get; set; }
        public bool allDay { get; set; }


        public Event(User_Shift_Schedule uss)
        {
            DateTime startDate = uss.Schedule.StartDate;
            startDate.Add(uss.Shift.Start);
            DateTime endDate = uss.Schedule.EndDate;
            endDate.Add(uss.Shift.End);
            title = String.Format("{0} {1} - {2}", uss.User.FirstName, uss.User.LastName, uss.Shift.JobRole);
            start = startDate;
            end = endDate;
            backgroundColor = "red";
            allDay = false;


        }

     
    }
}