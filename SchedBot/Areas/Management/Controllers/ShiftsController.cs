using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SchedBot.Areas.Management.Controllers
{
    public class ShiftsController : Controller
    {
        // GET: Management/Shifts
        public ActionResult Index()
        {
            return View();
        }
    }
}