﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace Deco2500HighFidelityPrototype.Models.ViewModels
{
    public class DietIndexViewModel
    {
        public DietIndexViewModel(User user)
        {
            User = user;
        }
        public User User { get; private set; }
    }
}
