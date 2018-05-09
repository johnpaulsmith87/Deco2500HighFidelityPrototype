using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Deco2500HighFidelityPrototype.Models
{
    public class Meal
    {
        public List<(Guid IngredientId, decimal weight)> IngredientsAndWeights { get; set; }
    }
}
