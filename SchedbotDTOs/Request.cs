using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchedbotDTOs
{
    public class Request
    {
        public int RequestId { get; set; }

        public string Reason { get; set; }

        public string Status { get; set; }

        public string StatusExplanation { get; set; }

        public string RequestType{ get; set; }

        public int OriginalShiftId { get; set; }
        public int TradingShiftId { get; set; }

        public DateTime? StartTimeOff { get; set; }
        public DateTime? EndTimeOff { get; set; }

        //Foreign Key
        public int SendingUserId { get; set; }
        public virtual User SendingUser { get; set; }

        public int ReceivingUserId { get; set; }
        public virtual User ReceivingUser { get; set; }

        public virtual Shift OriginalShift { get; set; }
        public virtual Shift TradingShift{ get; set; }


    }
}
