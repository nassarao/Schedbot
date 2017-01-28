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
using SchedBot.Models;
using SchedBot.Areas.Management.Models;

namespace SchedBot.Areas.Management.Controllers
{
    public class RequestsController : Controller
    {
        private SchedBotContext db = new SchedBotContext();

        // GET: Management/Requests
        public async Task<ActionResult> Index()
        {
            RequestViewModel vm = new RequestViewModel();
            vm.Requests = db.Requests.ToList();
            vm.RequestTypes = db.RequestTypes.ToList();

          
            return View(vm);
        }

        // GET: Management/Requests/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Request request = await db.Requests.FindAsync(id);
            if (request == null)
            {
                return HttpNotFound();
            }
            return View(request);
        }

        // GET: Management/Requests/Create
        public ActionResult Create()
        {
            ViewBag.RequestTypeId = new SelectList(db.RequestTypes, "RequestTypeId", "Name");
            return View();
        }

        // POST: Management/Requests/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "RequestId,Reason,Status,StatusExplanation,RequestTypeId")] Request request)
        {
            if (ModelState.IsValid)
            {
                db.Requests.Add(request);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.RequestTypeId = new SelectList(db.RequestTypes, "RequestTypeId", "Name", request.RequestTypeId);
            return View(request);
        }

        // GET: Management/Requests/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Request request = await db.Requests.FindAsync(id);
            if (request == null)
            {
                return HttpNotFound();
            }
            ViewBag.RequestTypeId = new SelectList(db.RequestTypes, "RequestTypeId", "Name", request.RequestTypeId);
            return View(request);
        }

        // POST: Management/Requests/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "RequestId,Reason,Status,StatusExplanation,RequestTypeId")] Request request)
        {
            if (ModelState.IsValid)
            {
                db.Entry(request).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.RequestTypeId = new SelectList(db.RequestTypes, "RequestTypeId", "Name", request.RequestTypeId);
            return View(request);
        }

        // GET: Management/Requests/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Request request = await db.Requests.FindAsync(id);
            if (request == null)
            {
                return HttpNotFound();
            }
            return View(request);
        }

        // POST: Management/Requests/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Request request = await db.Requests.FindAsync(id);
            db.Requests.Remove(request);
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
