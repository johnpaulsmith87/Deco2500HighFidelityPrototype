using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Deco2500HighFidelityPrototype.Models
{
    public class Ingredient
    {
        public Guid IngredientId { get; set; }
        public string Name { get; set; }
        public decimal CaloriesPerGram { get; set; }
    }
}
