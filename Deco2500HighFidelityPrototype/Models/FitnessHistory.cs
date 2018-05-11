using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Deco2500HighFidelityPrototype.Models
{
    public class FitnessHistory : IHistory
    {
        public Guid UserId { get; set; }
        public DateTime EventDateTime { get; set; }
        public Routine RoutinePerformed { get; set; }
    }
}
