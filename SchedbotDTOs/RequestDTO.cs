using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchedbotDTOs
{
    public class RequestDTO
    {
        public int RequestId { get; set; }

        public string Reason { get; set; }

        public string Status { get; set; }

        public string StatusExplanation { get; set; }

        //Foreign key
        public int RequestTypeId{ get; set; }
        public virtual RequestTypeDTO RequestType { get; set; }
    }

    
    public class RequestTypeDTO
    {
        public int RequestTypeId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }
    }
}
