using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Deco2500HighFidelityPrototype.Models
{
   public enum ExerciseType
    {
        RepBased,
        DistanceBased
    }
    [Flags]
    public enum ScreenContext
    {
        Home = 1 << 0,
        Fitness = 1 << 1,
        Diet = 1 << 2,
        Settings =  1 << 3,
        History = 1 << 4,
        AddMeal = 1 << 5,
        CanGoBack = 1 << 6       // |= if you can go back
    }
    public enum HistoryType
    {
        Fitness,
        Diet
    }
}
