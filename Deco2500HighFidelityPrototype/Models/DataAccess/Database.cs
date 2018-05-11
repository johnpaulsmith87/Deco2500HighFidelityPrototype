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
        private const int MAX_EXERCISE = 924;
        /// <summary>
        /// This method will seed a database if one does not exist. It will add ingredients/excercises and load a user with
        /// some initial history.
        /// </summary>
        /// <param name="appRoot"> File path to the root directory of the application. Generate using IHostingEnvironment through dependency injection.</param>
        public static async void SeedDatabase(string appRoot)
        {
            if (!File.Exists(Path.Combine(appRoot, DATABASE_FILE_PATH)))
            {
                //put all seeding code here!
                string exerciseText = await File.ReadAllTextAsync(Path.Combine(appRoot, EXERCISE_FILE_PATH));
                int exerciseNumber = 1;
                int lowIndex = 0;
                int highIndex = 0;
                while(exerciseNumber < MAX_EXERCISE)
                {


                    exerciseNumber++;
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
    }
    public class DatabaseModel
    {
        //put all datamodel classes in here
        public List<Exercise> AllExercises { get; set; } // these are loaded from a text file (plus additional seeds?)
        public List<User> Users { get; set; }
        public List<Ingredient> AllIngredients { get; set; } //same as exercises?
    }
}
