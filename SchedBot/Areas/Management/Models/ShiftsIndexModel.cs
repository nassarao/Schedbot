using SchedbotDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SchedBot.Areas.Management.Models
{
    public class ShiftsIndexModel
    {
        public List<Shift> Shifts{ get; set; }
        public List<JobRole> Roles{ get; set; }
        public List<SelectListItem> JobRoles { get; set;}

    }
}