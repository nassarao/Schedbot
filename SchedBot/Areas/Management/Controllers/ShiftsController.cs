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

namespace SchedBot.Areas.Management.Controllers
{
    public class ShiftsController : Controller
    {
        private SchedBotContext db = new SchedBotContext();
        ScheduleManager sm = new ScheduleManager();

        // GET: Management/Shifts
        public async Task<ActionResult> Index()
        {
            ShiftsIndexModel vm = new ShiftsIndexModel()
            {
                Roles = db.JobRoles.ToList(),
                Shifts = db.Shifts.OrderBy(x => x.Day).ThenBy(x => x.Start).ThenBy(x => x.End).ToList()

            };

            vm.JobRoles = new List<SelectListItem>();
            foreach (var role in vm.Roles)
            {
                vm.JobRoles.Add(new SelectListItem() { Text = role.Name, Value = role.JobRoleId.ToString() });
            }

            return View(vm);
        }

        // GET: Management/Shifts/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Shift shift = await db.Shifts.FindAsync(id);
            if (shift == null)
            {
                return HttpNotFound();
            }
            return View(shift);
        }

        // GET: Management/Shifts/Create
        public ActionResult Create()
        {

            return View();
        }

        // POST: Management/Shifts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]

        public async Task<ActionResult> Create(FormCollection coll)
        {
            Shift shift = new Shift()
            {
                JobRoleId = int.Parse(coll.GetValue("JobRoles").AttemptedValue),
                End = DateTime.Parse(coll.GetValue("End").AttemptedValue).TimeOfDay,
                Start = DateTime.Parse(coll.GetValue("Start").AttemptedValue).TimeOfDay,
                Day = (DayOfWeek)Enum.Parse(typeof(DayOfWeek), coll.GetValue("Day").AttemptedValue),
                Type = coll.GetValue("Type").AttemptedValue,
                Active = bool.Parse(coll.GetValue("Active").AttemptedValue)
            };
            if (ModelState.IsValid)
            {
                db.Shifts.Add(shift);
                await db.SaveChangesAsync();
                sm.DropAndCreateFutureSchedule();
                return RedirectToAction("Index");
            }


            return View(shift);
        }

      

        // GET: Management/Shifts/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Shift shift = await db.Shifts.FindAsync(id);
            if (shift == null)
            {
                return HttpNotFound();
            }

            return View(shift);
        }

        // POST: Management/Shifts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ShiftID,Date,ShiftTime,ShiftTypeId")] Shift shift)
        {
            if (ModelState.IsValid)
            {
                db.Entry(shift).State = EntityState.Modified;
                await db.SaveChangesAsync();
                sm.DropAndCreateFutureSchedule();
                return RedirectToAction("Index");
            }

            return View(shift);
        }

        // GET: Management/Shifts/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Shift shift = await db.Shifts.FindAsync(id);
            if (shift == null)
            {
                return HttpNotFound();
            }
            return View(shift);
        }

        // POST: Management/Shifts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Shift shift = await db.Shifts.FindAsync(id);
            db.Shifts.Remove(shift);
            await db.SaveChangesAsync();
            sm.DropAndCreateFutureSchedule();
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
