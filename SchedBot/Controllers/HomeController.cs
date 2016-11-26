using Microsoft.AspNet.Identity.EntityFramework;
using SchedBot.Models;
using SchedbotDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SchedBot.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
       
        public ActionResult Index()
        {

            

            using (var db = new SchedBotContext())
            {

                User user = new User { FirstName = "Ahamd"};
                db.Users.Add(user);
                db.SaveChanges();

              
            }
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
    }
}