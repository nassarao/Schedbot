using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using SchedbotDTOs;
using System.Threading.Tasks;

namespace SchedBot
{
    public class ScheduleManager
    {
        private SchedBotContext db = new SchedBotContext();

        public delegate Task ScheduleFinalizedEventArgs(object source, ScheduleFinalizeEventArgs args);

        //Define event
        public event ScheduleFinalizedEventArgs ScheduleFinalized;

        bool shiftAssigned = false;
        public async Task FinalizeSchedule()
        {
            MailService mail = new MailService();
            ScheduleFinalized += mail.OnScheduleFinalized;

            Schedule nextSched = db.Schedules.FirstOrDefault(x => x.Flag == Schedule.Flags.NotFinal);
            if (nextSched != null)
            {
                nextSched.Flag = Schedule.Flags.Final;
                db.Entry(nextSched).State = EntityState.Modified;
                db.SaveChanges();
                //Raising Schedule Finalized Event 
                await OnScheduleFinalized(nextSched.Id);
            }
        }



        protected virtual async Task OnScheduleFinalized(int schedId)
        {

            ScheduleFinalizeEventArgs args = new ScheduleFinalizeEventArgs()
            {

                Users = db.Users.Where(x => x.Email != null).ToList(),
                Schedule = db.Schedules.FirstOrDefault(x => x.Id == schedId)
            };
            await ScheduleFinalized?.Invoke(this, args);
        }
        
        public void DropAndCreateFutureSchedule()
        {
            Schedule future = db.Schedules.FirstOrDefault(x => x.Flag == Schedule.Flags.NotFinal);
            if (future != null)
            {
                db.Schedules.Remove(future);
                db.SaveChanges();
            }

            CreateSchedule();
            
        }

        public void CreateSchedule()
        {
            //To populate shifts that have no avilable users
            User unAssignedUser = db.Users.FirstOrDefault(x => x.FirstName == "Unassigned" && x.Email == null);

            // Creating an empty schedule with proper date ranges and sets flag
            Schedule sched;
            List<Shift> shifts;
            CreateScheduleWithShifts(out sched, out shifts);

            //Getting user availabilty and job roles
            List<User> users = db.Users.Include(x => x.Availability).ToList();
            List<User_JobRole> ujs = db.User_JobRoles.Include(x => x.User).Include(x => x.User.Availability).ToList();
            IDictionary<int, List<User_JobRole>> roleUserDic = new Dictionary<int, List<User_JobRole>>();


            //Populats roleUserDic This prevents us from having to make multiple db calls
            PopulateRoleUserDictionary(ujs, roleUserDic);

            //link users Id with hours working through the entire week
            Dictionary<int, double> currentHours = new Dictionary<int, double>();

            //looping through days of the week
            for (int i = 0; i < 7; i++)
                FillAllShiftsForThisDay(unAssignedUser, sched, shifts, roleUserDic, currentHours, i);


        }

        private void FillAllShiftsForThisDay(User unAssignedUser, Schedule sched, List<Shift> shifts, IDictionary<int, List<User_JobRole>> roleUserDic, Dictionary<int, double> currentHours, int i)
        {
            //Int is userID and the List of shifts he is working
            Dictionary<int, List<Shift>> userDayShifts = new Dictionary<int, List<Shift>>();

            //Getting all shifts for this day of week
            List<Shift> dayShifts = shifts.Where(x => x.Day == (DayOfWeek)Enum.Parse(typeof(DayOfWeek), i.ToString())).ToList();

            //Loop through each shift
            foreach (Shift shift in dayShifts)
            {
                shiftAssigned = false;
                if( shift.ShiftID == 20 )
                {

                }
                if (roleUserDic.ContainsKey(shift.JobRoleId))
                {
                    //All useres that can work this specific shift
                    List<User_JobRole> uj = roleUserDic[shift.JobRoleId];
                    bool isAm = shift.Start.Hours < 12;

                    //Looping through each user who is in this job role
                    FindAndAssignAUserWhoCanWorkThisShift(unAssignedUser, sched, currentHours, i, userDayShifts, shift, uj, isAm);

                }
                else
                {
                    AssignEmptyUserToShift(unAssignedUser, sched, shift);

                }
            }
        }

        private void FindAndAssignAUserWhoCanWorkThisShift(User unAssignedUser, Schedule sched, Dictionary<int, double> currentHours, int i, Dictionary<int, List<Shift>> userDayShifts, Shift shift, List<User_JobRole> uj, bool isAm)
        {
            foreach (User_JobRole user in uj)
            {
              
                //Determine if they are avilabe to work this shift
                int av = GetUserAvilabilty(i, user);

                //checking what users avilabilty is on that day way
                if (av != 3 && ((av != 1 && isAm) || (av != 2 && !isAm)))
                {
                    if (uj.IndexOf(user) >= uj.Count - 1)
                    {
                        AssignEmptyUserToShift(unAssignedUser, sched, shift);
                         break;
                    }
                }
                else
                {
                    //
                    bool isAvailable = true;
                    isAvailable = VerifyShiftsDontOverLap(userDayShifts, shift, user, isAvailable);

                    if (isAvailable)
                    {
                        //Getting length of a shift
                        double shiftHours, usercurrentHours;
                        GetLengthOfShift(shift, out shiftHours, out usercurrentHours);

                        if (currentHours.ContainsKey(user.UserId))
                            usercurrentHours = currentHours[user.UserId];

                        //Making sure user is working more than his requested max hours
                        if (usercurrentHours + shiftHours < user.User.Availability.MaxHours)
                        {
                            //If we made it this far this user can work this shift
                            AssignUserToShift(sched, userDayShifts, shift, user);
                            usercurrentHours = UpdateUsersHours(currentHours, user, shiftHours, usercurrentHours);
                            break;
                        }
                        else
                        {
                            //If we make it here that means there is no avilable users to work this shift. Assign an empty User to -1
                            if (uj.IndexOf(user) >= uj.Count - 1)
                            {
                                AssignEmptyUserToShift(unAssignedUser, sched, shift);
                                break;
                            }
                        }

                    }
                    else 
                        AssignEmptyUserToShift(unAssignedUser, sched, shift);

                }
            }
        }

        private static void PopulateRoleUserDictionary(List<User_JobRole> ujs, IDictionary<int, List<User_JobRole>> roleUserDic)
        {
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
        }

        private static bool VerifyShiftsDontOverLap(Dictionary<int, List<Shift>> userDayShifts, Shift shift, User_JobRole user, bool isAvailable)
        {
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

            return isAvailable;
        }

        private void AssignEmptyUserToShift(User unAssignedUser, Schedule sched, Shift shift)
        {
            if (shiftAssigned != true)
            {
                User_Shift_Schedule uss = new User_Shift_Schedule()
                {
                    ScheduleId = sched.Id,
                    UserId = unAssignedUser.UserId,
                    ShiftId = shift.ShiftID
                };
                db.UserShiftSchedules.Add(uss);
                db.SaveChanges();
                shiftAssigned = true;
            }
        }

        private static void GetLengthOfShift(Shift shift, out double shiftHours, out double usercurrentHours)
        {
            TimeSpan shiftHrs = shift.End - shift.Start;
            shiftHours = shiftHrs.Hours + shiftHrs.Minutes / 60;
            usercurrentHours = 0.0;
        }

        private static double UpdateUsersHours(Dictionary<int, double> currentHours, User_JobRole user, double shiftHours, double usercurrentHours)
        {

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

            return usercurrentHours;
        }

        private void AssignUserToShift(Schedule sched, Dictionary<int, List<Shift>> userDayShifts, Shift shift, User_JobRole user)
        {
            if (shiftAssigned != true)
            {
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
                shiftAssigned = true;
            }
        }

        private static int GetUserAvilabilty(int i, User_JobRole user)
        {
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

            return av;
        }

        private void CreateScheduleWithShifts(out Schedule sched, out List<Shift> shifts)
        {
            sched = new Schedule(db.Schedules.FirstOrDefault(x => x.Flag == Schedule.Flags.Current));
            db.Schedules.Add(sched);
            db.SaveChanges();

            //Assigning shifts that need to be filled in schedule
            shifts = db.Shifts.Where(x => x.Active == true).ToList();
            List<Schedule_Shift> ss = sched.PopulateSchedule(shifts);
            db.Schedule_Shifts.AddRange(ss);
            db.SaveChanges();
        }


    }
}