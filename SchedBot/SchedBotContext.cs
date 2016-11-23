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

        public System.Data.Entity.DbSet<SchedbotDTOs.AvailableShifts> AvailableShifts { get; set; }

        public System.Data.Entity.DbSet<SchedbotDTOs.RequestType> RequestTypes { get; set; }

        public System.Data.Entity.DbSet<SchedbotDTOs.ShiftType> ShiftTypes { get; set; }
    }
}