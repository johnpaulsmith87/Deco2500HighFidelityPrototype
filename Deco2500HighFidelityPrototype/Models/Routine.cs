using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Deco2500HighFidelityPrototype.Models
{
    public class Routine
    {
        public int RoutineId { get; set; }
        public List<(int ExerciseId, decimal amountTypeDependent, TimeSpan timeTaken)> Exercises { get; set; }
    }
}
