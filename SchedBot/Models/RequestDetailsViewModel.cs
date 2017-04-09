using SchedbotDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SchedBot.Models.Requests
{
    public class RequestDetailsViewModel
    {
        public User_Shift_Schedule original { get; set; }
        public User_Shift_Schedule trading { get; set; }
        public Request requestDTO { get; set; }
        public User sending { get; set; }

        public User receiving { get; set; }

        public RequestDetailsViewModel(Request re, SchedBotContext db)
        {
            requestDTO = re;
            original = db.UserShiftSchedules.Include("Shift").Include("Schedule").FirstOrDefault(x => x.Id == re.OriginalUSSId);
            trading = db.UserShiftSchedules.Include("Shift").Include("Schedule").FirstOrDefault(x => x.Id == re.TradingUSSId);
            sending = db.Users.FirstOrDefault(x => x.UserId == re.SendingUserId);
            receiving = db.Users.FirstOrDefault(x => x.UserId == re.ReceivingUserId);

        }
    }
}