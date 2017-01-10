using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SchedBot;
using SchedbotDTOs;
using SchedBot.Areas.Management.Models;
using SchedBot.Controllers;
using Microsoft.AspNet.Identity.Owin;
using SchedBot.Models;

namespace SchedBot.Areas.Management.Controllers
{
    public class UsersController : Controller
    {
        private SchedBotContext db = new SchedBotContext();

        // GET: Management/Users
        public async Task<ActionResult> Index()
        {
            UserIndexViewModel userIndexVM = new UserIndexViewModel();
            userIndexVM.UserDTOs = await db.Users.ToListAsync();
            return View(userIndexVM);
        }

        // GET: Management/Users/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = await db.Users.FindAsync(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // GET: Management/Users/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Management/Users/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(UserIndexViewModel vm)
        {
            if (ModelState.IsValid)
            {
                vm.NewUser.Email = vm.RegisterVM.Email;

                db.Users.Add(vm.NewUser);
                await db.SaveChangesAsync();

                var user = new ApplicationUser { UserName = vm.RegisterVM.Email, Email = vm.RegisterVM.Email };
                ApplicationUserManager userManger = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
                var result = await userManger.CreateAsync(user, vm.RegisterVM.Password);
                if (result.Succeeded)
                {
                    vm.NewUser.AccountId = user.Id;
                   await db.SaveChangesAsync();
                    return RedirectToAction("Details", "Users", new { id = vm.NewUser.UserId, area = "Management" });
                }
                else
                {
                    db.Users.Remove(vm.NewUser);
                    foreach (var item in result.Errors)
                    {
                        ModelState.AddModelError("errors", item);
                    }
                }

            }
            ModelState.AddModelError("errors", "Bad Model returned");
            return RedirectToAction("Index");
        }

        // GET: Management/Users/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = await db.Users.FindAsync(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Management/Users/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "UserId,FirstName,LastName,Address,City,State,ZipCode")] User user)
        {
            if (ModelState.IsValid)
            {
                db.Entry(user).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(user);
        }

        // GET: Management/Users/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = await db.Users.FindAsync(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Management/Users/Delete/5
        [HttpPost, ActionName("Delete")]
        // [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            User user = await db.Users.FindAsync(id);
            db.Users.Remove(user);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }


        [HttpPost]
        public bool SaveAvailability(FormCollection collection)
        {
            foreach (var item in collection.AllKeys)
            {
                string[] name = item.Split('-');
                string dayOfWeek = name[0];
                string shift = name[1];

                switch (dayOfWeek)
                {
                    case "sun":
                        amPmBoth(shift);
                        break;
                    case "mon":
                        break;
                    case "tue":
                        break;
                    case "wed":
                        break;
                    case "thu":
                        break;
                    case "fri":
                        break;
                    case "sat":
                        break;

                }
            }


            return true;
        }

        private int amPmBoth(string data)
        {
            switch (data)
            {
                case "am":
                    return 1;

                case "pm":
                    return 2;

                case "both":
                    return 3;

                default:
                    return 0;
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
