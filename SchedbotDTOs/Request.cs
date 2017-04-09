using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
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

        public int OriginalUSSId { get; set; }
        public int TradingUSSId { get; set; }

        public DateTime? StartTimeOff { get; set; }
        public DateTime? EndTimeOff { get; set; }

        //Foreign Key
        public int SendingUserId { get; set; }
        public int ReceivingUserId { get; set; }

        //[ForeignKey("SendingUserId")]
        //public User SendingUser { get; set; }
        //[ForeignKey("ReceivingUserId")]
        //public User ReceivingUser { get; set; }

        //[ForeignKey("OriginalShiftId")]
        //public Shift OriginalShift { get; set; }
        //[ForeignKey("TradingShiftId")]
        //public Shift TradingShift { get; set; }



    }
}
