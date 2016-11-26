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
    public class ShiftTypesController : Controller
    {
        private SchedBotContext db = new SchedBotContext();

        // GET: Management/ShiftTypes
        public async Task<ActionResult> Index()
        {
            return View(await db.ShiftTypes.ToListAsync());
        }

        // GET: Management/ShiftTypes/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ShiftType shiftType = await db.ShiftTypes.FindAsync(id);
            if (shiftType == null)
            {
                return HttpNotFound();
            }
            return View(shiftType);
        }

        // GET: Management/ShiftTypes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Management/ShiftTypes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ShiftTypeId,Name,Description")] ShiftType shiftType)
        {
            if (ModelState.IsValid)
            {
                db.ShiftTypes.Add(shiftType);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(shiftType);
        }

        // GET: Management/ShiftTypes/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ShiftType shiftType = await db.ShiftTypes.FindAsync(id);
            if (shiftType == null)
            {
                return HttpNotFound();
            }
            return View(shiftType);
        }

        // POST: Management/ShiftTypes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ShiftTypeId,Name,Description")] ShiftType shiftType)
        {
            if (ModelState.IsValid)
            {
                db.Entry(shiftType).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(shiftType);
        }

        // GET: Management/ShiftTypes/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ShiftType shiftType = await db.ShiftTypes.FindAsync(id);
            if (shiftType == null)
            {
                return HttpNotFound();
            }
            return View(shiftType);
        }

        // POST: Management/ShiftTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            ShiftType shiftType = await db.ShiftTypes.FindAsync(id);
            db.ShiftTypes.Remove(shiftType);
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
