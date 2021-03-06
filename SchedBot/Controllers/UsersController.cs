﻿using System;
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
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using SchedBot.Models;


namespace SchedBot.Controllers
{
    [Authorize]

    public class UsersController : Controller
    {
        private SchedBotContext db = new SchedBotContext();
        ScheduleManager sm = new ScheduleManager();

        // GET: Management/Users
        [Authorize(Roles ="Manager")]
        public async Task<ActionResult> Index()
        {
            UserIndexViewModel userIndexVM = new UserIndexViewModel();
            userIndexVM.UserDTOs = await db.Users.Where(x => x.FirstName != "Unassigned" && x.Email != null).Include("Availability").OrderBy(x => x.FirstName).ThenBy(x => x.LastName).ToListAsync();
            return View(userIndexVM);
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


                var user = new ApplicationUser { UserName = vm.RegisterVM.Email, Email = vm.RegisterVM.Email };
                ApplicationUserManager userManger = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
                var result = await userManger.CreateAsync(user, vm.RegisterVM.Password);
                if (result.Succeeded)
                {
                    vm.NewUser.Email = vm.RegisterVM.Email;
                    vm.NewUser.Availability = new Availability();
                    vm.NewUser.AccountId = user.Id;
                    db.Users.Add(vm.NewUser);
                    await db.SaveChangesAsync();

                    return RedirectToAction("Edit", "Users", new { id = vm.NewUser.UserId, area = "" });
                }
                else
                {
                    //db.Availability.Remove(vm.NewUser.Availability);
                    //db.Users.Remove(vm.NewUser);
                    foreach (var item in result.Errors)
                    {
                        ModelState.AddModelError("errors", item);
                    }
                }

            }
            ModelState.AddModelError("errors", "Bad Model returned");
            return RedirectToAction("Index", "Management/Users");
        }

        // GET: Management/Users/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Include(x => x.Availability).FirstOrDefault(x => x.UserId == id);
            if (user == null)
            {
                return HttpNotFound();
            }
            UserEditViewModel vm = new UserEditViewModel();
            vm.user = user;
            vm.jobRoles = db.JobRoles.ToList();

            vm.user_jobRoles = db.User_JobRoles.Where(x => x.UserId == user.UserId).ToList();
            vm.CheckBoxProps = new Dictionary<JobRole, bool>();
            foreach (var item in vm.jobRoles)
            {
                vm.CheckBoxProps.Add(item, false);
                foreach (var uj in vm.user_jobRoles)
                {
                    if (uj.JobRoleId == item.JobRoleId)
                    {
                        vm.CheckBoxProps[item] = true;

                    }
                }
            }
            return View(vm);
        }

        [HttpPost]
        public ActionResult LoadRoles(int id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Include(x => x.Availability).FirstOrDefault(x => x.UserId == id);
            if (user == null)
            {
                return HttpNotFound();
            }
            UserEditViewModel vm = new UserEditViewModel();
            vm.user = user;
            vm.jobRoles = db.JobRoles.ToList();

            vm.user_jobRoles = db.User_JobRoles.Where(x => x.UserId == user.UserId).ToList();
            vm.CheckBoxProps = new Dictionary<JobRole, bool>();
            foreach (var item in vm.jobRoles)
            {
                vm.CheckBoxProps.Add(item, false);
                foreach (var uj in vm.user_jobRoles)
                {
                    if (uj.JobRoleId == item.JobRoleId)
                    {
                        vm.CheckBoxProps[item] = true;

                    }
                }
            }
            return PartialView("RolesPartial",vm);
        }

        //GET: Management/Users/MyProfile
        public RedirectToRouteResult MyProfile()
        {
            var userEmail = User.Identity;
            var schedUser = db.Users.Where(x => x.Email == userEmail.Name).FirstOrDefault();
            return RedirectToAction("Edit", new { id = schedUser.UserId });
        }

        // POST: Management/Users/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "UserId,FirstName,LastName,Address,City,State,ZipCode,Email,PhoneNumber,AvailabilityId,AccountId")] User user)
        {
            if (ModelState.IsValid)
            {
            ApplicationUserManager userManger = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
                var aspUser = userManger.FindById(user.AccountId);
                aspUser.Email = user.Email;
                aspUser.UserName = user.Email;
                userManger.Update(aspUser);

                db.Entry(user).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(user);
        }


        // POST: Management/Users/Delete/5
        [HttpPost , ActionName("Delete")]
        // [ValidateAntiForgeryToken]
        [Authorize(Roles ="Manager")]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            User user = await db.Users.FindAsync(id);
            db.Users.Remove(user);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        [HttpPost]
        [Authorize(Roles ="Manager")]
        public async Task<ActionResult> SaveRoles(FormCollection collection)
        {
            int UserId = int.Parse(collection["user.UserId"]);
            User user = db.Users.Find(UserId);
            db.User_JobRoles.RemoveRange(db.User_JobRoles.Where(x => x.UserId == UserId));
            List<User_JobRole> ujs = new List<User_JobRole>();

            foreach (var item in collection.AllKeys)
            {
                try
                {
                    int jobRoleId = int.Parse(item);
                    User_JobRole newUJ = db.User_JobRoles.FirstOrDefault(x => x.JobRoleId == jobRoleId && x.UserId == user.UserId);
                    
                    
                        User_JobRole uj = new User_JobRole()
                        {
                            UserId = UserId,
                            JobRoleId = jobRoleId
                        };
                        ujs.Add(uj);

                    
                }
                catch (Exception ex){
                }
            }
            if (ujs.Count > 0)
                db.User_JobRoles.AddRange(ujs);


            await db.SaveChangesAsync();
            sm.DropAndCreateFutureSchedule();
            return RedirectToAction("Index", "Users");

        }

        [HttpPost]
        public async Task<ActionResult> SaveAvailability(FormCollection collection)
        {
            int userId = int.Parse(collection["UserId"]);
            User user = db.Users.Find(userId);



            foreach (var item in collection.AllKeys)
            {
                var colValue = collection.GetValue(item);
                if (item.Contains("MaxHours"))
                {
                    user.Availability.MaxHours = int.Parse(colValue.AttemptedValue);
                }
                if (item.Contains("Availability"))
                {
                    string[] name = item.Split('-');

                    string dayOfWeek = item.Split('.')[1];



                    switch (dayOfWeek)
                    {
                        case "Sunday":
                            user.Availability.Sunday = int.Parse(colValue.AttemptedValue);
                            break;
                        case "Monday":
                            user.Availability.Monday = int.Parse(colValue.AttemptedValue);
                            break;
                        case "Tuesday":
                            user.Availability.Tuesday = int.Parse(colValue.AttemptedValue);
                            break;
                        case "Wednesday":
                            user.Availability.Wednesday = int.Parse(colValue.AttemptedValue);
                            break;
                        case "Thursday":
                            user.Availability.Thursday = int.Parse(colValue.AttemptedValue);
                            break;
                        case "Friday":
                            user.Availability.Friday = int.Parse(colValue.AttemptedValue);
                            break;
                        case "Saturday":
                            user.Availability.Saturday = int.Parse(colValue.AttemptedValue);
                            break;

                    }
                }
            }

            db.Entry(user).State = EntityState.Modified;
            await db.SaveChangesAsync();
            sm.DropAndCreateFutureSchedule();

            return RedirectToAction("Edit", new { id = userId });
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
