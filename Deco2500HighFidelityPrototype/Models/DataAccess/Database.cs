using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Deco2500HighFidelityPrototype.Models;
using System.IO;

namespace Deco2500HighFidelityPrototype.Models.DataAccess
{
    /// <summary>
    /// This static class acts as a read/write to the database. It will also include an inital seeder.
    /// </summary>
    public static class Database
    {


    }
    public class DatabaseModel
    {
        //put all datamodel classes in here
        public List<Exercise> AllExercises { get; set; } // these are loaded from a text file (plus additional seeds?)
        public List<User> Users { get; set; }
        public List<Ingredient> AllIngredients { get; set; } //same as exercises?
    }
}
