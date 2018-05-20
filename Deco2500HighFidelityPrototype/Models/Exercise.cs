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
        public Exercise(string name, decimal caloriesPerUnit, int id)
        {
            Name = name.TrimEnd();
            CaloriesPerUnit = caloriesPerUnit;
            ExerciseId = id;
            if (DistanceTypeExcercises().Contains(name))
                ExerciseType = ExerciseType.DistanceBased;
            else
                ExerciseType = ExerciseType.RepBased;
        }
        public string Name { get; set; }
        // needs type and duration?? maybe something else?
        public ExerciseType ExerciseType { get; set; }

        public decimal CaloriesPerUnit { get; set; }

        public int ExerciseId { get; set; }

        private static List<string> DistanceTypeExcercises() => new List<string>() {
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

