﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Deco2500HighFidelityPrototype.Models.ViewModels
{
    public class HomeIndexViewModel
    {
        public HomeIndexViewModel(User user)
        {
            User = user;
        }
        public User User { get; private set; }
    }
}