using SchedbotDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SchedBot.Areas.Management.Models
{
    public class RequestViewModel
    {

        public List<Request> Requests{ get; set; }

        public List<RequestType> RequestTypes { get; set; }
    }
}