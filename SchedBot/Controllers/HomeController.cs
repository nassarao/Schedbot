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

        [HttpGet]
        public JsonResult GenerateScheduleJSON()
        {
            DateTime oneYearAgo = DateTime.Now.AddYears(-1);
            List<Schedule> scheds = db.Schedules.Where(x => x.StartDate >= oneYearAgo.Date).ToList();
            CalanderScheduleViewModel vm = new CalanderScheduleViewModel();
            vm.events = new List<Event>();

            if (scheds != null && scheds.Count >= 1)
            {
                foreach (Schedule sched in scheds)
                {

                    List<User_Shift_Schedule> ussL = db.UserShiftSchedules.Where(x => x.ScheduleId == sched.Id).ToList();

                    foreach (User_Shift_Schedule uss in ussL)
                        vm.events.Add(new Event(uss));
                }
                return Json(vm.events, JsonRequestBehavior.AllowGet);
            }


            return null;
        }
    }
}