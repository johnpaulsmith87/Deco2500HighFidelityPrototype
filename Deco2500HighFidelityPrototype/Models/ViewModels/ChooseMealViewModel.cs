using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Deco2500HighFidelityPrototype.Controllers;

namespace Deco2500HighFidelityPrototype.Models.ViewModels
{
    public class ChooseMealViewModel
    {
        public List<DietHistoryGraphItem> SuggestedMeals { get; set; }
        public List<DietHistoryGraphItem> HistoricalMeals { get; set; }
    }
}
