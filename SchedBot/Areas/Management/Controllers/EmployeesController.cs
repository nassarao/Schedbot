using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SchedBot.Areas.Management.Controllers
{
    public class EmployeesController : Controller
    {
        // GET: Management/Employees
        public ActionResult Index()
        {
            return View();
        }
    }
}