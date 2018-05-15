using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Deco2500HighFidelityPrototype.Models.DataAccess;
using Deco2500HighFidelityPrototype.Models;

namespace Deco2500HighFidelityPrototype.Services
{
    /// <summary>
    /// Singleton class for application caching - use to display
    /// Exercises and Ingredients in the front end to avoid
    /// database reading (i.e slow disk reads)
    /// </summary>
    public class ApplicationStateService
    {


        public ApplicationStateService(string appRoot)
        {
            var db = Database.GetDatabase(appRoot);
            AllIngredients = db.AllIngredients;
            AllExercises = db.AllExercises;
        }

        public List<Ingredient> AllIngredients { get; private set; }
        public List<Exercise> AllExercises { get; private set; }

    }
}
