using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Deco2500HighFidelityPrototype.Models
{
    public class Routine
    {
        public Guid RoutineId { get; set; }
        public List<(Guid ExerciseId, decimal amountTypeDependent)> Exercises { get; set; }
    }
}
