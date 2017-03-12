using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchedbotDTOs
{
    public class Shift
    {

        public int ShiftID { get; set; }

        public DayOfWeek Day { get; set; }

        public TimeSpan Start { get; set; }
        public TimeSpan End { get; set; }
        public bool Active { get; set; }
        public string Type { get; set; }


        public int JobRoleId { get; set; }
        public virtual JobRole JobRole { get; set; }
    }
}
