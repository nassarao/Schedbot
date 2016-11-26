﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SchedbotDTOs;
using SchedBot.Models;

namespace SchedBot.Areas.Management.Models
{
    public class UserIndexViewModel
    {
        public List<User> UserDTOs { get; set; }

        public User NewUser { get; set; }

        public RegisterViewModel RegisterVM { get; set; }

    }
}