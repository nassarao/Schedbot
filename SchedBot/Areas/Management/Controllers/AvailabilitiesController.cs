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

namespace SchedBot.Areas.Management.Controllers
{
    public class AvailabilitiesController : Controller
    {
        private SchedBotContext db = new SchedBotContext();

        // GET: Management/Availabilities
        public async Task<ActionResult> Index()
        {
            return View(await db.Availability.ToListAsync());
        }

        // GET: Management/Availabilities/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Availability availability = await db.Availability.FindAsync(id);
            if (availability == null)
            {
                return HttpNotFound();
            }
            return View(availability);
        }

        // GET: Management/Availabilities/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Management/Availabilities/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "AvailabilityId,Sunday,Monday,Tuesday,Wednesday,Thursday,Friday,Saturday")] Availability availability)
        {
            if (ModelState.IsValid)
            {
                db.Availability.Add(availability);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(availability);
        }

        // GET: Management/Availabilities/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Availability availability = await db.Availability.FindAsync(id);
            if (availability == null)
            {
                return HttpNotFound();
            }
            return View(availability);
        }

        // POST: Management/Availabilities/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "AvailabilityId,Sunday,Monday,Tuesday,Wednesday,Thursday,Friday,Saturday")] Availability availability)
        {
            if (ModelState.IsValid)
            {
                db.Entry(availability).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(availability);
        }

        // GET: Management/Availabilities/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Availability availability = await db.Availability.FindAsync(id);
            if (availability == null)
            {
                return HttpNotFound();
            }
            return View(availability);
        }

        // POST: Management/Availabilities/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Availability availability = await db.Availability.FindAsync(id);
            db.Availability.Remove(availability);
            await db.SaveChangesAsync();
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
