using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Deco2500HighFidelityPrototype.Models
{
    public class DietHistory : IHistory
    {
        public Guid UserId { get; set; }
        public DateTime EventDateTime { get; set; }
        public Meal Meal { get; set; }
    }
}
