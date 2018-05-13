using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Deco2500HighFidelityPrototype.Models
{
    public class Exercise
    {
        public Exercise()
        {
            // empty default constructor for serialization/deserialization
        }
        public Exercise(string name, decimal caloriesPerUnit)
        {
            Name = name.Trim();
            CaloriesPerUnit = caloriesPerUnit;
            if (DistanceTypeExcercises().Contains(name))
                ExerciseType = ExerciseType.DistanceBased;
            else
                ExerciseType = ExerciseType.RepBased;
        }
        public string Name { get; set; }
        // needs type and duration?? maybe something else?
        public ExerciseType ExerciseType { get; set; }

        public decimal CaloriesPerUnit { get; set; }

        public Guid ExerciseId { get; set; }

        private static List<string> DistanceTypeExcercises()
        {
            return new List<string>()
            {
               "Swim Backstroke",
               "Swim Breastroke",
               "Swim Butterfly",
               "Swim Freestyle",
               "Walk Continuous or Program",
               "Run Mixed Terrain",
               "Run Treadmill"

            };
        }


    }
}

