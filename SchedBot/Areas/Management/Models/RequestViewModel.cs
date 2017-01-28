﻿using SchedbotDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SchedBot.Areas.Management.Models
{
    public class RequestViewModel
    {

        public List<Request> Requests{ get; set; }

        public string RequestType { get; set; }

        public List<SelectListItem> Users { get; set; }
        public List<SelectListItem> ReqTypes { get; set; }
    }
}