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
            var rand = new Random();
            if (!File.Exists(Path.Combine(appRoot, DATABASE_FILE_PATH)))
            {
                try
                {
                    //generate ingredient/exercise database from files
                    exerciseLines = await File.ReadAllLinesAsync(Path.Combine(appRoot, EXERCISE_FILE_PATH));
                    ingredientLines = await File.ReadAllLinesAsync(Path.Combine(appRoot, INGREDIENT_FILE_PATH));
                    var seedExercises = exerciseLines.Select(e => new Exercise(e, GenerateRandomCaloriesPerUnit(rand)));
                    var seedIngredients = ingredientLines.Distinct().Select(i => new Ingredient(i, GenerateRandomCaloriesPerGram(rand)));
                    // now we can create a new user and fake history - then we save to db
                    var user = new User()
                    {
                        DOB = DateTime.Parse("1996-06-06"),
                        Name = "Cornelius Von Hammerschmidt",
                        Id = Guid.NewGuid(),
                        Preferences = Preferences.GetDefaultPreferences()
                    };
                    //  fake history, since Guids have just been generated (we don't know them!)
                    // I'll have to query the ingredients/exercises by name and get their guid that way 
                    //  first let's gather up like 5-6 ingredients and 2-3 exercises!
                    // this code is not efficient (i.e repeating work) but it really doesn't matter for this application

                    //ingredients i will use
                    var chicken = seedIngredients.First(i => i.Name == "chicken");
                    var broccoli = seedIngredients.First(i => i.Name == "broccoli");
                    var turkey = seedIngredients.First(i => i.Name == "turkey");
                    var bacon = seedIngredients.First(i => i.Name == "bacon");
                    var lettuce = seedIngredients.First(i => i.Name == "iceberg lettuce");
                    var rice = seedIngredients.First(i => i.Name == "rice");
                    var bread = seedIngredients.First(i => i.Name == "bread");
                    var mayonnaise = seedIngredients.First(i => i.Name == "mayonnaise");

                    //exercises
                    var runTreadmill = seedExercises.First(e => e.Name.Contains("Run Treadmill"));
                    var pushUp = seedExercises.First(e => e.Name.Contains("Push Up"));
                    var curl = seedExercises.First(e => e.Name.Contains("Curl & Press Dumbbells"));

                    var meal1 = new Meal()
                    {
                        MealId = Guid.NewGuid(),
                        IngredientsAndWeights = new List<(Guid IngredientId, decimal weightInGrams)>()
                        {
                            (chicken.IngredientId, 240.0m),
                            (broccoli.IngredientId, 100.5m),
                            (rice.IngredientId, 205.5m)
                        }
                    };
                    var meal2 = new Meal()
                    {
                        MealId = Guid.NewGuid(),
                        IngredientsAndWeights = new List<(Guid IngredientId, decimal weightInGrams)>()
                        {
                            (turkey.IngredientId, 150.0m),
                            (bacon.IngredientId, 80.5m),
                            (bread.IngredientId, 150.5m),
                            (mayonnaise.IngredientId, 25m)
                        }
                    };
                    var meal3 = new Meal()
                    {
                        MealId = Guid.NewGuid(),
                        IngredientsAndWeights = new List<(Guid IngredientId, decimal weightInGrams)>()
                        {
                            (chicken.IngredientId, 240.0m),
                            (broccoli.IngredientId, 100.5m),
                            (rice.IngredientId, 205.5m)
                        }
                    };

                    var routine1 = new Routine()
                    {
                        RoutineId = Guid.NewGuid(),
                        Exercises = new List<(Guid ExerciseId, decimal amountTypeDependent, TimeSpan timeTaken)>()
                        {
                            (runTreadmill.ExerciseId, 5000m, TimeSpan.Parse("00:22:34")),
                            (pushUp.ExerciseId, 100m, TimeSpan.Parse("00:10:30"))
                        }
                    };
                    var routine2 = new Routine()
                    {
                        RoutineId = Guid.NewGuid(),
                        Exercises = new List<(Guid ExerciseId, decimal amountTypeDependent, TimeSpan timeTaken)>()
                        {
                            (runTreadmill.ExerciseId, 5000m, TimeSpan.Parse("00:23:15")),
                            (pushUp.ExerciseId, 100m, TimeSpan.Parse("00:09:57")),
                            (curl.ExerciseId, 30m, TimeSpan.Parse("00:06:23"))
                        }
                    };
                    //Now use these to create histories
                    user.History = new List<IHistory>
                    {
                        new DietHistory() { UserId = user.Id, EventDateTime = DateTime.Now - TimeSpan.FromDays(3), Meal = meal1 },
                        new DietHistory() { UserId = user.Id, EventDateTime = DateTime.Now - TimeSpan.FromDays(2), Meal = meal2 },
                        new DietHistory() { UserId = user.Id, EventDateTime = DateTime.Now - TimeSpan.FromDays(1), Meal = meal3 },
                        new FitnessHistory() { UserId = user.Id, EventDateTime = DateTime.Now - TimeSpan.FromDays(3), RoutinePerformed = routine1 },
                        new FitnessHistory() { UserId = user.Id, EventDateTime = DateTime.Now - TimeSpan.FromDays(1), RoutinePerformed = routine2 }
                    };
                    //create database object
                    var database = new DatabaseModel()
                    {
                        Users = new List<User>() { user },
                        AllExercises = seedExercises.ToList(),
                        AllIngredients = seedIngredients.ToList()
                    };
                    //save to file~ fingers crossed!
                    await SaveDatabase(database, appRoot);
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
        public static async void SaveDatabase(DatabaseModel dbToSave, string appRoot)
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
