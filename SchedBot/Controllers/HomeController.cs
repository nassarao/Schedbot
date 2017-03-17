using Microsoft.AspNet.Identity.EntityFramework;
using SchedBot.Areas.Management.Models;
using SchedBot.Models;
using SchedbotDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace SchedBot.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private SchedBotContext db = new SchedBotContext();

        public ActionResult Index()
        {

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public JsonResult GenerateScheduleJSON(DateTime start, DateTime end)
        {
            Schedule sched = db.Schedules.FirstOrDefault(x => x.StartDate == start && x.EndDate == end);
            if (sched != null) {
                List<User_Shift_Schedule> ussL = db.UserShiftSchedules.Where(x => x.ScheduleId == sched.Id).ToList();
                CalanderScheduleViewModel vm = new CalanderScheduleViewModel();
                vm.events = new List<Event>();
                foreach (User_Shift_Schedule uss in ussL)
                {
                    vm.events.Add(new Event(uss));
                }
                return Json(new JavaScriptSerializer().Serialize(vm.events));
            }
            return null;
        }
    }
}