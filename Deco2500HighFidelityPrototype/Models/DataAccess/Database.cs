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
        public static void SeedDatabase(string appRoot)
        {
            string[] exerciseLines;
            string[] ingredientLines;
            var rand = new Random();
            if (File.Exists(Path.Combine(appRoot, DATABASE_FILE_PATH)))
            {
                return;
            }
            try
            {
                //generate ingredient/exercise database from files
                exerciseLines = File.ReadAllLines(Path.Combine(appRoot, EXERCISE_FILE_PATH));
                ingredientLines = File.ReadAllLines(Path.Combine(appRoot, INGREDIENT_FILE_PATH));
                var seedExercises = exerciseLines.Select(e => new Exercise(e, GenerateRandomCaloriesPerUnit(rand))).ToList();
                var seedIngredients = ingredientLines.Select(i => new Ingredient(i, GenerateRandomCaloriesPerGram(rand))).Distinct().ToList();
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
                var beef = seedIngredients.First(i => i.Name == "beef");
                var beans = seedIngredients.First(i => i.Name == "baked beans");
                var salmon = seedIngredients.First(i => i.Name == "salmon");
                var cereal = seedIngredients.First(i => i.Name == "cereal");
                var milk = seedIngredients.First(i => i.Name == "milk");

                //exercises
                var runTreadmill = seedExercises.First(e => e.Name.Contains("Run Treadmill"));
                var pushUp = seedExercises.First(e => e.Name.Contains("Push Up"));
                var curl = seedExercises.First(e => e.Name.Contains("Curl & Press Dumbbells"));
                var swim = seedExercises.First(e => e.Name == "Swim Freestyle");
                var chinUp = seedExercises.First(e => e.Name == "Chin Up");

                var meal1 = new Meal()
                {
                    MealId = Guid.NewGuid(),
                    IngredientsAndWeights = new List<(Guid IngredientId, decimal weightInGrams)>()
                        {
                            (chicken.IngredientId, 240.0m),
                            (broccoli.IngredientId, 100.5m),
                            (rice.IngredientId, 205.5m)
                        },
                    Name = "Chicken, broccoli and rice"
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
                        },
                    Name = "Turkey BLT"
                };
                var meal3 = new Meal()
                {
                    MealId = Guid.NewGuid(),
                    IngredientsAndWeights = new List<(Guid IngredientId, decimal weightInGrams)>()
                        {
                            (cereal.IngredientId, 100m),
                            (milk.IngredientId, 200m)
                        },
                    Name = "Breakfast cereal with milk"
                };
                var meal4 = new Meal()
                {
                    MealId = Guid.NewGuid(),
                    IngredientsAndWeights = new List<(Guid IngredientId, decimal weightInGrams)>()
                        {
                            (cereal.IngredientId, 100m),
                            (milk.IngredientId, 200m)
                        },
                    Name = "Breakfast cereal with milk"
                };
                var meal5 = new Meal()
                {
                    MealId = Guid.NewGuid(),
                    IngredientsAndWeights = new List<(Guid IngredientId, decimal weightInGrams)>()
                        {
                            (beef.IngredientId, 200m),
                            (rice.IngredientId, 100m),
                            (beans.IngredientId, 50m)
                        },
                    Name = "Beef, beans and rice"
                };
                var meal6 = new Meal()
                {
                    MealId = Guid.NewGuid(),
                    IngredientsAndWeights = new List<(Guid IngredientId, decimal weightInGrams)>()
                        {
                            (salmon.IngredientId, 200m),
                            (broccoli.IngredientId, 100m),
                            (beans.IngredientId, 50m)
                        },
                    Name = "Salmon, broccoli and beans"
                };
                var meal7 = new Meal()
                {
                    MealId = Guid.NewGuid(),
                    IngredientsAndWeights = new List<(Guid IngredientId, decimal weightInGrams)>()
                        {
                            (cereal.IngredientId, 100m),
                            (milk.IngredientId, 200m)
                        },
                    Name = "Breakfast cereal with milk"
                };
                var routine1 = new Routine()
                {
                    RoutineId = Guid.NewGuid(),
                    Exercises = new List<(Guid ExerciseId, decimal amountTypeDependent, TimeSpan timeTaken)>()
                        {
                            (runTreadmill.ExerciseId, 5000m, TimeSpan.Parse("00:22:34")),
                            (pushUp.ExerciseId, 100m, TimeSpan.Parse("00:10:30"))
                        },
                    Name = "Run and pushups - the usual"
                };
                var routine2 = new Routine()
                {
                    RoutineId = Guid.NewGuid(),
                    Exercises = new List<(Guid ExerciseId, decimal amountTypeDependent, TimeSpan timeTaken)>()
                        {
                            (runTreadmill.ExerciseId, 5000m, TimeSpan.Parse("00:23:15")),
                            (pushUp.ExerciseId, 100m, TimeSpan.Parse("00:09:57")),
                            (curl.ExerciseId, 30m, TimeSpan.Parse("00:06:23"))
                        },
                    Name = "the usual plus bicep curls"
                };
                var routine3 = new Routine()
                {
                    RoutineId = Guid.NewGuid(),
                    Exercises = new List<(Guid ExerciseId, decimal amountTypeDependent, TimeSpan timeTaken)>()
                        {
                            (runTreadmill.ExerciseId, 5000m, TimeSpan.Parse("00:23:15")),
                            (swim.ExerciseId, 1000m, TimeSpan.Parse("00:20:57"))
                        },
                    Name = "Swim and a run"
                };
                //Now use these to create histories
                //wrote this code around ~3 pm... should test around that time too lol
                user.History = new List<IHistory>
                    {
                        new DietHistory() {UserId = user.Id, EventDateTime = DateTime.Now - TimeSpan.FromDays(2) - TimeSpan.FromHours(7), Meal = meal3},
                        new DietHistory() {UserId = user.Id, EventDateTime = DateTime.Now - TimeSpan.FromDays(2) - TimeSpan.FromHours(3), Meal = meal2},
                        new DietHistory() {UserId = user.Id, EventDateTime = DateTime.Now - TimeSpan.FromDays(2) + TimeSpan.FromHours(3), Meal = meal1},
                        new DietHistory() {UserId = user.Id, EventDateTime = DateTime.Now - TimeSpan.FromDays(1) - TimeSpan.FromHours(7), Meal = meal4},
                        new DietHistory() {UserId = user.Id, EventDateTime = DateTime.Now - TimeSpan.FromDays(1) - TimeSpan.FromHours(3), Meal = meal6},
                        new DietHistory() {UserId = user.Id, EventDateTime = DateTime.Now - TimeSpan.FromDays(1) + TimeSpan.FromHours(3), Meal = meal5},
                        new DietHistory() {UserId = user.Id, EventDateTime = DateTime.Now - TimeSpan.FromHours(7), Meal = meal7},
                        new FitnessHistory() { UserId = user.Id, EventDateTime = DateTime.Now - TimeSpan.FromDays(2), RoutinePerformed = routine3 },
                        new FitnessHistory() { UserId = user.Id, EventDateTime = DateTime.Now - TimeSpan.FromDays(1), RoutinePerformed = routine2 },
                        new FitnessHistory() { UserId = user.Id, EventDateTime = DateTime.Now - TimeSpan.FromHours(2), RoutinePerformed = routine1 }
                    };
                //create database object
                var database = new DatabaseModel()
                {
                    Users = new List<User>() { user },
                    AllExercises = seedExercises.ToList(),
                    AllIngredients = seedIngredients.ToList()
                };
                //save to file~ fingers crossed! - code smell!
                SaveDatabase(database, appRoot);
            }
            catch (Exception e)
            {
                //do nothing, just don't cause an uncaught exception
            }


        }
        /// <summary>
        /// Returns an instance of DatabaseModel with data parsed from database.json
        /// </summary>
        /// <param name="appRoot"> Pass in _env.ContentRootPath as the appRoot string parameter</param>
        /// <returns></returns>
        public static DatabaseModel GetDatabase(string appRoot)
        {
            var rawText = File.ReadAllText(Path.Combine(appRoot, DATABASE_FILE_PATH));
            return JsonConvert.DeserializeObject<DatabaseModel>(rawText, new JsonSerializerSettings()
            {
                TypeNameHandling = TypeNameHandling.Auto
            });
        }
        /// <summary>
        /// Saves the given DatabaseModel instance to file using the json format.
        /// </summary>
        /// <param name="dbToSave"> DatabaseModel to save to disk</param>
        /// <param name="appRoot">Pass in _env.ContentRootPath as the appRoot string parameter</param>
        public static void SaveDatabase(DatabaseModel dbToSave, string appRoot)
        {
            var jsonString = JsonConvert.SerializeObject(dbToSave, Formatting.Indented, new JsonSerializerSettings()
            {
                TypeNameHandling = TypeNameHandling.Auto
            });
            File.WriteAllText(Path.Combine(appRoot, DATABASE_FILE_PATH), jsonString);
        }
        private static decimal GenerateRandomCaloriesPerGram(Random rand)
        {
            return (decimal)(rand.NextDouble() * 5);
        }
        private static decimal GenerateRandomCaloriesPerUnit(Random rand)
        {
            return (decimal)(rand.NextDouble() * 3);
        }
    }
    /// <summary>
    /// This class represents the database for the application. Use this class to read/write to database.json
    /// </summary>
    public class DatabaseModel
    {
        /// <summary>
        /// List of all exercises read from allExercises.txt (>800)
        /// </summary>
        public List<Exercise> AllExercises { get; set; }
        /// <summary>
        /// List of application Users. There's no constraint on uniqueness so be careful not to add additional
        /// users. (We only need one user for this application).
        /// </summary>
        public List<User> Users { get; set; }
        /// <summary>
        /// List of all exercises read from allIngredients.txt (>400)
        /// </summary>
        public List<Ingredient> AllIngredients { get; set; }
    }
}
