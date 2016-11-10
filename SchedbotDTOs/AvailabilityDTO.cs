using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchedbotDTOs
{
    public class AvailabilityDTO
    {

        public int AvilabilityId { get; set; }

        public int Sunday { get; set; }
        public int Monday { get; set; }
        public int Tuesday { get; set; }
        public int Wednesday { get; set; }
        public int Thursday { get; set; }
        public int Friday { get; set; }
        public int Saturday { get; set; }

        //todo: Uncomment this depending how we link our account tables
        //public int AccountId { get; set; }
        // public virtual AccountDTO Account { get; set; }
    }

    public class AvailableShiftsDTO
    {
        //this will be populated with AM,PM,Third,Both etc...
        public int AvailableShiftsId { get; set; }
        public string Name { get; set; }
    }
}
