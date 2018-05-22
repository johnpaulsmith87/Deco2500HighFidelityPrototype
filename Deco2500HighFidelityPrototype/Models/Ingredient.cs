using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Deco2500HighFidelityPrototype.Models
{
    public class Ingredient : IEquatable<Ingredient>
    {
        public Ingredient()
        {
            //empty constructor for serialization/deserialization
        }
        public Ingredient(string name, decimal caloriesPerGram)
        {
            //use some random math to assign calories per gram
            Name = name;
            CaloriesPerGram = caloriesPerGram;
            IngredientId = Guid.NewGuid();
        }
        public Guid IngredientId { get; set; }
        public string Name { get; set; }
        public decimal CaloriesPerGram { get; set; }

        public bool Equals(Ingredient other)
        {
            return Name == other.Name;
        }
    }
}
