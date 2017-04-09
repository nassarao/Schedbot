using SchedbotDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SchedBot.Models
{
    public class SchedulesIndexModel
    {
        public List<User> UserDTOs { get; set; }
        public List<Schedule> Schedules { get; set; }
        public List<User_Shift_Schedule> UserShiftScheduleDTOs { get; set; }
    }
}