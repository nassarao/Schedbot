using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchedbotDTOs
{
    public class ShiftDTO
    {

        public int ShiftID { get; set; }

        public DateTime Date { get; set; }

        public TimeSpan ShiftTime { get; set; }

        //Foreign Key
        public int ShiftTypeId { get; set; }
        public virtual ShiftTypeDTO ShiftType { get; set; }

    }

    public class ShiftTypeDTO
    {
        public int ShiftTypeId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }
    }
}
