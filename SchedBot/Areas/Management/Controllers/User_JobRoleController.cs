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
    [Authorize]

    public class User_JobRoleController : Controller
    {
        private SchedBotContext db = new SchedBotContext();

        // GET: Management/User_JobRole
        public async Task<ActionResult> Index()
        {
            var user_JobRoles = db.User_JobRoles.Include(u => u.JobRole).Include(u => u.User);
            return View(await user_JobRoles.ToListAsync());
        }

        // GET: Management/User_JobRole/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User_JobRole user_JobRole = await db.User_JobRoles.FindAsync(id);
            if (user_JobRole == null)
            {
                return HttpNotFound();
            }
            return View(user_JobRole);
        }

        // GET: Management/User_JobRole/Create
        public ActionResult Create()
        {
            ViewBag.JobRoleId = new SelectList(db.JobRoles, "JobRoleId", "Name");
            ViewBag.UserId = new SelectList(db.Users, "UserId", "FirstName");
            return View();
        }

        // POST: Management/User_JobRole/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "User_JobRoleId,JobRoleId,UserId")] User_JobRole user_JobRole)
        {
            if (ModelState.IsValid)
            {
                db.User_JobRoles.Add(user_JobRole);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.JobRoleId = new SelectList(db.JobRoles, "JobRoleId", "Name", user_JobRole.JobRoleId);
            ViewBag.UserId = new SelectList(db.Users, "UserId", "FirstName", user_JobRole.UserId);
            return View(user_JobRole);
        }

        // GET: Management/User_JobRole/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User_JobRole user_JobRole = await db.User_JobRoles.FindAsync(id);
            if (user_JobRole == null)
            {
                return HttpNotFound();
            }
            ViewBag.JobRoleId = new SelectList(db.JobRoles, "JobRoleId", "Name", user_JobRole.JobRoleId);
            ViewBag.UserId = new SelectList(db.Users, "UserId", "FirstName", user_JobRole.UserId);
            return View(user_JobRole);
        }

        // POST: Management/User_JobRole/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "User_JobRoleId,JobRoleId,UserId")] User_JobRole user_JobRole)
        {
            if (ModelState.IsValid)
            {
                db.Entry(user_JobRole).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.JobRoleId = new SelectList(db.JobRoles, "JobRoleId", "Name", user_JobRole.JobRoleId);
            ViewBag.UserId = new SelectList(db.Users, "UserId", "FirstName", user_JobRole.UserId);
            return View(user_JobRole);
        }

        // GET: Management/User_JobRole/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User_JobRole user_JobRole = await db.User_JobRoles.FindAsync(id);
            if (user_JobRole == null)
            {
                return HttpNotFound();
            }
            return View(user_JobRole);
        }

        // POST: Management/User_JobRole/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            User_JobRole user_JobRole = await db.User_JobRoles.FindAsync(id);
            db.User_JobRoles.Remove(user_JobRole);
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
