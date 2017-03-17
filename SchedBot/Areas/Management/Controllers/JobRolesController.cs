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
    public class JobRolesController : Controller
    {
        private SchedBotContext db = new SchedBotContext();

        // GET: Management/JobRoles
        public async Task<ActionResult> Index()
        {
            return View(await db.JobRoles.ToListAsync());
        }

        // GET: Management/JobRoles/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            JobRole jobRole = await db.JobRoles.FindAsync(id);
            if (jobRole == null)
            {
                return HttpNotFound();
            }
            return View(jobRole);
        }

        // GET: Management/JobRoles/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Management/JobRoles/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "JobRoleId,Name,Description")] JobRole jobRole)
        {
            if (ModelState.IsValid)
            {
                db.JobRoles.Add(jobRole);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(jobRole);
        }

        // GET: Management/JobRoles/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            JobRole jobRole = await db.JobRoles.FindAsync(id);
            if (jobRole == null)
            {
                return HttpNotFound();
            }
            return View(jobRole);
        }

        // POST: Management/JobRoles/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "JobRoleId,Name,Description")] JobRole jobRole)
        {
            if (ModelState.IsValid)
            {
                db.Entry(jobRole).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(jobRole);
        }

        // GET: Management/JobRoles/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            JobRole jobRole = await db.JobRoles.FindAsync(id);
            if (jobRole == null)
            {
                return HttpNotFound();
            }
            return View(jobRole);
        }

        // POST: Management/JobRoles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            JobRole jobRole = await db.JobRoles.FindAsync(id);
            db.JobRoles.Remove(jobRole);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }


        [AcceptVerbs(HttpVerbs.Get)]
        public JsonResult GetListOfRoles()
        {
            var listOfRoles = Json(db.JobRoles.ToList());
            var rolesJSON = Json(listOfRoles, JsonRequestBehavior.AllowGet);
            return rolesJSON;
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
