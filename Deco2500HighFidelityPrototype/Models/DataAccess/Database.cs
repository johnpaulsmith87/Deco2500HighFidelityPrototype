using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Deco2500HighFidelityPrototype.Models;
using System.IO;
using Newtonsoft.Json;
using System.Text.RegularExpressions;

namespace Deco2500HighFidelityPrototype.Models.DataAccess
{
    /// <summary>
    /// This static class acts as a read/write to the database. It will also include an inital seeder.
    /// </summary>
    public static class Database
    {
        private const string EXERCISE_FILE_PATH = "./allExercises.txt";
        private const string DATABASE_FILE_PATH = "./database.json";
        private const string INGREDIENT_FILE_PATH = "./allIngredients.txt";
        /// <summary>
        /// This method will seed a database if one does not exist. It will add ingredients/excercises and load a user with
        /// some initial history.
        /// </summary>
        /// <param name="appRoot"> File path to the root directory of the application. Generate using IHostingEnvironment through dependency injection.</param>
        public static async void SeedDatabase(string appRoot)
        {
            string[] exerciseLines;
            string[] ingredientLines;
            if (!File.Exists(Path.Combine(appRoot, DATABASE_FILE_PATH)))
            {
                try
                {
                    var rand = new Random();

                    exerciseLines = await File.ReadAllLinesAsync(Path.Combine(appRoot, EXERCISE_FILE_PATH));
                    ingredientLines = await File.ReadAllLinesAsync(Path.Combine(appRoot, INGREDIENT_FILE_PATH));

                    var seedExercises = exerciseLines.Select(e => new Exercise(e, GenerateRandomCaloriesPerUnit(rand)));
                    var seedIngredients = ingredientLines.Select(i => new Ingredient(i, GenerateRandomCaloriesPerGram(rand)));
                    var test = false;

                }
                catch (Exception e)
                {
                    //do nothing, just don't cause an uncaught exception
                }
            }

        }
        public static DatabaseModel GetDatabase(string appRoot)
        {
            throw new NotImplementedException();
        }
        public static void SaveDatabase(DatabaseModel dbToSave, string appRoot)
        {

        }
        private static decimal GenerateRandomCaloriesPerGram(Random rand)
        {
            return (decimal)(rand.NextDouble() * 10);
        }
        private static decimal GenerateRandomCaloriesPerUnit(Random rand)
        {
            return (decimal)(rand.NextDouble() * 7);
        }
    }
    public class DatabaseModel
    {
        //put all datamodel classes in here
        public List<Exercise> AllExercises { get; set; } // these are loaded from a text file (plus additional seeds?)
        public List<User> Users { get; set; }
        public List<Ingredient> AllIngredients { get; set; } //same as exercises?
    }
}
