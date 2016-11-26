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

        public DateTime Date { get; set; }

        public TimeSpan ShiftTime { get; set; }

        //Foreign Key
        public int ShiftTypeId { get; set; }
        public virtual ShiftType ShiftType { get; set; }

    }

    public class ShiftType
    {
       
        public int ShiftTypeId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }
    }
}
