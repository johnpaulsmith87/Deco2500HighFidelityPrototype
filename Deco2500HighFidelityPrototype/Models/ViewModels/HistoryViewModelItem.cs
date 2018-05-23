using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Deco2500HighFidelityPrototype.Models.ViewModels
{
    /// <summary>
    /// To be sent to History index view in a list!
    /// </summary>
    public class HistoryViewModelItem
    {
        public DateTime DateTime { get; set; }
        public string Name { get; set; }
        public HistoryType Type { get; set; }
        public decimal Calories { get; set; }
        public TimeSpan TimeSpent { get; set; }

    }
}
