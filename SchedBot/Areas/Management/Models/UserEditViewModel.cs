using SchedbotDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SchedBot.Areas.Management.Models
{
    public class UserEditViewModel
    {
        public User  user { get; set; }
        public List<AvailableShifts> shifts { get; set; }
        public Availability availability { get; set; }
        public List<User_JobRole> user_jobRoles { get; set; }
        public List<JobRole> jobRoles { get; set; }
        public Dictionary<JobRole,bool> CheckBoxProps { get; set; }
    }
}