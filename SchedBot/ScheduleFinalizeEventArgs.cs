using SchedbotDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SchedBot
{
    public class ScheduleFinalizeEventArgs :EventArgs
    {
      
            public Schedule Schedule { get; set; }
            public List<User> Users { get; set; }

        
    }
}