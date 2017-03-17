using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SchedBot;
using SchedbotDTOs;

namespace SchedBot.Areas.Management.Controllers
{
    public class SchedulesController : Controller
    {
        private SchedBotContext db = new SchedBotContext();

        // GET: Management/Schedules
        public ActionResult Index()
        {
            //Change Flag to Current for loading the default page...using NotFinal for building html purpose and testing
            var currentSchedule = db.Schedules.Where(x => x.Flag == Schedule.Flags.NotFinal);
            int currentScheduleId = currentSchedule.First().Id;
            var userShiftSchedules = db.UserShiftSchedules.Where(x => x.ScheduleId == currentScheduleId).Include(u => u.Schedule).Include(u => u.Shift).Include(u => u.User).OrderBy(x => x.Shift.Day).ThenBy(x => x.Shift.Start).ThenBy(x => x.Shift.End);
            return View(userShiftSchedules.ToList());
        }

        // GET: Management/Schedules/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User_Shift_Schedule user_Shift_Schedule = db.UserShiftSchedules.Find(id);
            if (user_Shift_Schedule == null)
            {
                return HttpNotFound();
            }
            return View(user_Shift_Schedule);
        }

        // GET: Management/Schedules/Create
        public ActionResult Create()
        {
            ViewBag.ScheduleId = new SelectList(db.Schedules, "Id", "Id");
            ViewBag.ShiftId = new SelectList(db.Shifts, "ShiftID", "Type");
            ViewBag.UserId = new SelectList(db.Users, "UserId", "Email");
            return View();
        }

        // POST: Management/Schedules/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Create(FormCollection coll)
        {

            Shift shift = new Shift()
            {
                JobRoleId = int.Parse(coll.GetValue("JobRoles").AttemptedValue),
                End = DateTime.Parse(coll.GetValue("End").AttemptedValue).TimeOfDay,
                Start = DateTime.Parse(coll.GetValue("Start").AttemptedValue).TimeOfDay,
                Day = (DayOfWeek)Enum.Parse(typeof(DayOfWeek), coll.GetValue("Day").AttemptedValue)
            };

            //generate new user dto here




            
            
            //if (ModelState.IsValid)
            //{
            //    db.UserShiftSchedules.Add(user_Shift_Schedule);
            //    db.SaveChanges();
            //    return RedirectToAction("Index");
            //}

            //ViewBag.ScheduleId = new SelectList(db.Schedules, "Id", "Id", user_Shift_Schedule.ScheduleId);
            //ViewBag.ShiftId = new SelectList(db.Shifts, "ShiftID", "Type", user_Shift_Schedule.ShiftId);
            //ViewBag.UserId = new SelectList(db.Users, "UserId", "Email", user_Shift_Schedule.UserId);
            return RedirectToAction("Index");
        }

        // GET: Management/Schedules/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User_Shift_Schedule user_Shift_Schedule = db.UserShiftSchedules.Find(id);
            if (user_Shift_Schedule == null)
            {
                return HttpNotFound();
            }
            ViewBag.ScheduleId = new SelectList(db.Schedules, "Id", "Id", user_Shift_Schedule.ScheduleId);
            ViewBag.ShiftId = new SelectList(db.Shifts, "ShiftID", "Type", user_Shift_Schedule.ShiftId);
            ViewBag.UserId = new SelectList(db.Users, "UserId", "Email", user_Shift_Schedule.UserId);
            return View(user_Shift_Schedule);
        }

        // POST: Management/Schedules/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,ShiftId,ScheduleId,UserId")] User_Shift_Schedule user_Shift_Schedule)
        {
            if (ModelState.IsValid)
            {
                db.Entry(user_Shift_Schedule).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ScheduleId = new SelectList(db.Schedules, "Id", "Id", user_Shift_Schedule.ScheduleId);
            ViewBag.ShiftId = new SelectList(db.Shifts, "ShiftID", "Type", user_Shift_Schedule.ShiftId);
            ViewBag.UserId = new SelectList(db.Users, "UserId", "Email", user_Shift_Schedule.UserId);
            return View(user_Shift_Schedule);
        }

        // GET: Management/Schedules/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User_Shift_Schedule user_Shift_Schedule = db.UserShiftSchedules.Find(id);
            if (user_Shift_Schedule == null)
            {
                return HttpNotFound();
            }
            return View(user_Shift_Schedule);
        }

        // POST: Management/Schedules/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            User_Shift_Schedule user_Shift_Schedule = db.UserShiftSchedules.Find(id);
            db.UserShiftSchedules.Remove(user_Shift_Schedule);
            db.SaveChanges();
            return RedirectToAction("Index");
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
