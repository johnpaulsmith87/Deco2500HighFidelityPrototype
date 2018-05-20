﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Deco2500HighFidelityPrototype.Models
{
    public class Ingredient
    {
        public Ingredient()
        {
            //empty constructor for serialization/deserialization
        }
        public Ingredient(string name, decimal caloriesPerGram, int id)
        {
            //use some random math to assign calories per gram
            Name = name;
            CaloriesPerGram = caloriesPerGram;
            IngredientId = id;
        }
        public int IngredientId { get; set; }
        public string Name { get; set; }
        public decimal CaloriesPerGram { get; set; }
    }
}
