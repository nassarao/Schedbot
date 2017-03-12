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

        // GET: Management/Shifts
        public async Task<ActionResult> Index()
        {
            ShiftsIndexModel vm = new ShiftsIndexModel()
            {
                Roles = db.JobRoles.ToList(),
                Shifts = db.Shifts.ToList()

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
                return RedirectToAction("Index");
            }


            return View(shift);
        }

        public ActionResult CreateSchedule()
        {


            Schedule sched = new Schedule(db.Schedules.FirstOrDefault(x => x.Flag == Schedule.Flags.Current));
            db.Schedules.Add(sched);
            db.SaveChanges();
            List<Shift> shifts = db.Shifts.Where(x => x.Active == true).ToList();
            List<Schedule_Shift> ss = sched.PopulateSchedule(shifts);
            db.Schedule_Shifts.AddRange(ss);
            db.SaveChanges();

            List<User> users = db.Users.Include(x => x.Availability).ToList();
            List<User_JobRole> ujs = db.User_JobRoles.Include(x => x.User).Include(x => x.User.Availability).ToList();
            IDictionary<int, List<User_JobRole>> roleUserDic = new Dictionary<int, List<User_JobRole>>();

            foreach (User_JobRole uj in ujs)
            {
                //foreach (JobRole role in user.)
                if (roleUserDic.ContainsKey(uj.JobRoleId))
                {
                    roleUserDic[uj.JobRoleId].Add(uj);
                }
                else
                {
                    List<User_JobRole> ujRole = new List<User_JobRole>();
                    ujRole.Add(uj);
                    roleUserDic.Add(uj.JobRoleId, ujRole);
                }
            }
            Dictionary<int, double> currentHours = new Dictionary<int, double>();
            for (int i = 0; i < 7; i++)
            {
                Dictionary<int, List<Shift>> userDayShifts = new Dictionary<int, List<Shift>>();


                List<Shift> dayShifts = shifts.Where(x => x.Day == (DayOfWeek)Enum.Parse(typeof(DayOfWeek), i.ToString())).ToList();
                foreach (Shift shift in dayShifts)
                {
                    List<User_JobRole> uj = roleUserDic[shift.JobRoleId];
                    bool isAm = shift.Start.Hours < 12;
                    foreach (User_JobRole user in uj)
                    {

                        int av = -1;
                        switch (i)
                        {
                            case 0:
                                av = user.User.Availability.Sunday;
                                break;
                            case 1:
                                av = user.User.Availability.Monday;

                                break;
                            case 2:
                                av = user.User.Availability.Tuesday;

                                break;
                            case 3:
                                av = user.User.Availability.Wednesday;

                                break;
                            case 4:
                                av = user.User.Availability.Thursday;

                                break;
                            case 5:
                                av = user.User.Availability.Friday;

                                break;
                            case 6:
                                av = user.User.Availability.Saturday;

                                break;


                        }

                        if (av != 3 && ((av != 1 && isAm) || (av != 2 && !isAm)))
                            continue;
                        else
                        {

                            bool isAvailable = true;
                            if (userDayShifts.ContainsKey(user.UserId))
                            {
                                List<Shift> otherUserShifts = userDayShifts[user.UserId];
                                foreach (Shift os in otherUserShifts)
                                {

                                    if (os.Start < shift.Start)
                                    {
                                        if (os.End > shift.Start)
                                        {
                                            isAvailable = false;
                                        }
                                    }
                                    else
                                    {
                                        if (shift.End > os.Start)
                                        {
                                            isAvailable = false;
                                        }
                                    }
                                }
                            }

                            if (isAvailable)
                            {

                                TimeSpan shiftHrs = shift.End - shift.Start;
                                double shiftHours = shiftHrs.Hours + shiftHrs.Minutes / 60;
                                double usercurrentHours = 0.0;
                                if (currentHours.ContainsKey(user.UserId))
                                {
                                    usercurrentHours = currentHours[user.UserId];
                                }
                                if (usercurrentHours + shiftHours < user.User.Availability.MaxHours)
                                {

                                    User_Shift_Schedule uss = new User_Shift_Schedule()
                                    {
                                        ScheduleId = sched.Id,
                                        UserId = user.UserId,
                                        ShiftId = shift.ShiftID
                                    };

                                    db.UserShiftSchedules.Add(uss);
                                    db.SaveChanges();

                                    if (userDayShifts.ContainsKey(user.UserId))
                                    {
                                        userDayShifts[user.UserId].Add(shift);


                                    }
                                    else
                                    {
                                        List<Shift> userShiftList = new List<Shift>();
                                        userShiftList.Add(shift);
                                        userDayShifts.Add(user.UserId, userShiftList);
                                    }


                                    usercurrentHours += shiftHours;
                                    if (currentHours.ContainsKey(user.UserId))
                                    {

                                        currentHours[user.UserId] = usercurrentHours;

                                    }
                                    else
                                    {
                                        currentHours.Add(user.UserId, shiftHours);
                                    }


                                    break;
                                }

                            }
                        }
                    }

                }

            }


            return RedirectToAction("Index");
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
