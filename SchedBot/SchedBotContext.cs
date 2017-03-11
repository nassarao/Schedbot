using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using SchedbotDTOs;

namespace SchedBot
{


    public class SchedBotContext: DbContext
    {
        public DbSet<Availability> Availability { get; set; }
        public DbSet<Request> Requests { get; set; }
        public DbSet<Shift> Shifts { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<JobRole> JobRoles{ get; set; }
        public DbSet<User_JobRole> User_JobRoles{ get; set; }
        public DbSet<Schedule> Schedules{ get; set; }
        public DbSet<Schedule_Shift> Schedule_Shifts{ get; set; }
        public DbSet<AvailableShifts> AvailableShifts { get; set; }

        
    }
}