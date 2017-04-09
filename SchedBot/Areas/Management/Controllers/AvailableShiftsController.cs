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
    [Authorize(Roles ="Manager")]
    public class AvailableShiftsController : Controller
    {
        private SchedBotContext db = new SchedBotContext();

        // GET: Management/AvailableShifts
        public async Task<ActionResult> Index()
        {
            return View(await db.AvailableShifts.ToListAsync());
        }

        // GET: Management/AvailableShifts/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AvailableShifts availableShifts = await db.AvailableShifts.FindAsync(id);
            if (availableShifts == null)
            {
                return HttpNotFound();
            }
            return View(availableShifts);
        }

        // GET: Management/AvailableShifts/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Management/AvailableShifts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "AvailableShiftsId,Name")] AvailableShifts availableShifts)
        {
            if (ModelState.IsValid)
            {
                db.AvailableShifts.Add(availableShifts);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(availableShifts);
        }

        // GET: Management/AvailableShifts/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AvailableShifts availableShifts = await db.AvailableShifts.FindAsync(id);
            if (availableShifts == null)
            {
                return HttpNotFound();
            }
            return View(availableShifts);
        }

        // POST: Management/AvailableShifts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "AvailableShiftsId,Name")] AvailableShifts availableShifts)
        {
            if (ModelState.IsValid)
            {
                db.Entry(availableShifts).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(availableShifts);
        }

        // GET: Management/AvailableShifts/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AvailableShifts availableShifts = await db.AvailableShifts.FindAsync(id);
            if (availableShifts == null)
            {
                return HttpNotFound();
            }
            return View(availableShifts);
        }

        // POST: Management/AvailableShifts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            AvailableShifts availableShifts = await db.AvailableShifts.FindAsync(id);
            db.AvailableShifts.Remove(availableShifts);
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
