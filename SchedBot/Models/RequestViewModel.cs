using SchedBot.Models.Requests;
using SchedbotDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SchedBot.Models
{
    public class RequestViewModel
    {

        public List<RequestDetailsViewModel> Requests{ get; set; }
        //public List<Request> Requests { get; set; }


        public string RequestType { get; set; }

        public List<SelectListItem> Users { get; set; }
        public List<SelectListItem> ReqTypes { get; set; }
        public List<SelectListItem> UserShifts { get; set; }
        public List<SelectListItem> OtherUserShifts { get; set; }
        public int selectedUserId { get; set; }

    }
}