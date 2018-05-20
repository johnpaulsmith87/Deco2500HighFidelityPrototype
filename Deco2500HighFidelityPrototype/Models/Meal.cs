using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Deco2500HighFidelityPrototype.Models
{
    public class Meal
    {
        public int MealId { get; set; }
        public List<(int IngredientId, decimal weightInGrams)> IngredientsAndWeights { get; set; }
    }
}
