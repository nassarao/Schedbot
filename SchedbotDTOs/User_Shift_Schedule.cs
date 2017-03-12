using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchedbotDTOs
{
    public class User_Shift_Schedule
    {
        public int Id { get; set; }
        public int ShiftId { get; set; }
        public int ScheduleId { get; set; }
        public int UserId { get; set; }

        public virtual User User { get; set; }
        public virtual Shift Shift { get; set; }
        public virtual Schedule Schedule { get; set; }
    }
}
