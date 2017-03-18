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
            Schedule currentSchedule = db.Schedules.Where(x => x.Flag == Schedule.Flags.NotFinal).FirstOrDefault();
            if (currentSchedule != null)
            {
               
                List<User_Shift_Schedule>  userShiftSchedules = db.UserShiftSchedules.Where(x => x.ScheduleId == currentSchedule.Id)
                    .Include(u => u.Schedule).Include(u => u.Shift).Include(u => u.User)
                    .OrderBy(x => x.Shift.Day).ThenBy(x => x.Shift.Start).ThenBy(x => x.Shift.End).ToList();
                return View(userShiftSchedules);
            }
            return View();
        }

        public ActionResult CreateSchedule()
        {
            //To populate shifts that have no avilable users
            User unAssignedUser = db.Users.FirstOrDefault(x => x.FirstName == "Unassigned" && x.Email == null);

            // Creating an empty schedule with proper date ranges and sets flag
            Schedule sched = new Schedule(db.Schedules.FirstOrDefault(x => x.Flag == Schedule.Flags.Current));
            db.Schedules.Add(sched);
            db.SaveChanges();

            //Assigning shifts that need to be filled in schedule
            List<Shift> shifts = db.Shifts.Where(x => x.Active == true).ToList();
            List<Schedule_Shift> ss = sched.PopulateSchedule(shifts);
            db.Schedule_Shifts.AddRange(ss);
            db.SaveChanges();

            //Getting user availabilty and job roles
            List<User> users = db.Users.Include(x => x.Availability).ToList();
            List<User_JobRole> ujs = db.User_JobRoles.Include(x => x.User).Include(x => x.User.Availability).ToList();

            //all Jo
            IDictionary<int, List<User_JobRole>> roleUserDic = new Dictionary<int, List<User_JobRole>>();


            //Populats roleUserDic
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

            //link users Id with hours working through the entire week
            Dictionary<int, double> currentHours = new Dictionary<int, double>();

            //looping through days of the week
            for (int i = 0; i < 7; i++)
            {

                //Int is userID and the List of shifts he is working
                Dictionary<int, List<Shift>> userDayShifts = new Dictionary<int, List<Shift>>();

                //Getting all shifts for this day of week
                List<Shift> dayShifts = shifts.Where(x => x.Day == (DayOfWeek)Enum.Parse(typeof(DayOfWeek), i.ToString())).ToList();

                //Loop through each shift
                foreach (Shift shift in dayShifts)
                {

                    if (roleUserDic.ContainsKey(shift.JobRoleId))
                    {
                        //All useres that can work this specific shift
                        List<User_JobRole> uj = roleUserDic[shift.JobRoleId];
                        bool isAm = shift.Start.Hours < 12;

                        //Looping through each user who is in this job role
                        foreach (User_JobRole user in uj)
                        {

                            //Determine if they are avilabe to work this shift
                            int av = -1;
                            //I is day of week 
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

                            //checking what users avilabilty is on that day way
                            if (av != 3 && ((av != 1 && isAm) || (av != 2 && !isAm)))
                            {
                                //Not available to work
                                //if(uj.IndexOf(user) >= uj.Count)
                                //{

                                //}

                                continue;
                            }
                            else
                            {
                                //
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
                                    //Getting length of a shift
                                    TimeSpan shiftHrs = shift.End - shift.Start;
                                    double shiftHours = shiftHrs.Hours + shiftHrs.Minutes / 60;
                                    double usercurrentHours = 0.0;
                                    if (currentHours.ContainsKey(user.UserId))
                                    {
                                        usercurrentHours = currentHours[user.UserId];
                                    }

                                    //Making sure user is working more than his requested max hours
                                    if (usercurrentHours + shiftHours < user.User.Availability.MaxHours)
                                    {
                                        //If we made it this far this user can work this shift
                                        User_Shift_Schedule uss = new User_Shift_Schedule()
                                        {
                                            ScheduleId = sched.Id,
                                            UserId = user.UserId,
                                            ShiftId = shift.ShiftID
                                        };

                                        db.UserShiftSchedules.Add(uss);
                                        db.SaveChanges();

                                        //Making sure usr is or is not in list
                                        if (userDayShifts.ContainsKey(user.UserId))
                                        {
                                            userDayShifts[user.UserId].Add(shift);


                                        }
                                        else
                                        {
                                            //If we make it here that means he has no prior shifts this schedule and this is his first
                                            List<Shift> userShiftList = new List<Shift>();
                                            userShiftList.Add(shift);
                                            userDayShifts.Add(user.UserId, userShiftList);
                                        }

                                        //Updating a users working hours
                                        usercurrentHours += shiftHours;
                                        //Checking a dictionary if user is working this schedule
                                        if (currentHours.ContainsKey(user.UserId))
                                        {
                                            //user is working so we append a more time this week because he has multiple shifts 
                                            currentHours[user.UserId] = usercurrentHours;

                                        }
                                        else
                                        {
                                            //FIrst shift this schedule adding him to dictionary
                                            currentHours.Add(user.UserId, shiftHours);
                                        }


                                        break;
                                    }
                                    else
                                    {
                                        //If we make it here that means there is no avilable users to work this shift. Assign an empty User to -1


                                        if (uj.IndexOf(user) >= uj.Count - 1)
                                        {
                                            User_Shift_Schedule uss = new User_Shift_Schedule()
                                            {
                                                ScheduleId = sched.Id,
                                                UserId = unAssignedUser.UserId,
                                                ShiftId = shift.ShiftID
                                            };
                                            db.UserShiftSchedules.Add(uss);
                                            db.SaveChanges();
                                        }
                                    }

                                }

                            }
                        }

                    }
                    else
                    {
                        User_Shift_Schedule uss = new User_Shift_Schedule()
                        {
                            ScheduleId = sched.Id,
                            UserId = unAssignedUser.UserId,
                            ShiftId = shift.ShiftID
                        };
                        db.UserShiftSchedules.Add(uss);
                        db.SaveChanges();
                    }
                }
            }


            return RedirectToAction("Index");
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
