using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchedbotDTOs
{
    public class User_JobRole
    {

        public int User_JobRoleId { get; set; }
        public int JobRoleId { get; set; }
        public int UserId { get; set; }

        public virtual JobRole JobRole { get; set; }
        public virtual User User { get; set; }
    }
}
