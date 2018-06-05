using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Deco2500HighFidelityPrototype.Controllers;

namespace Deco2500HighFidelityPrototype.Models.ViewModels
{
    public class ExerciseIndexViewModel
    {
        public User User { get; set; }
        public FitnessHistoryGraphItem Today {get;set;}
        public bool ActiveToday { get; set; }
    }
}
