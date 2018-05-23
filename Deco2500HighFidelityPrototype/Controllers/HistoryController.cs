using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Deco2500HighFidelityPrototype.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Deco2500HighFidelityPrototype.Models;
using Deco2500HighFidelityPrototype.Models.ViewModels;
using Deco2500HighFidelityPrototype.Models.DataAccess;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Deco2500HighFidelityPrototype.Controllers
{
    public class HistoryController : Controller
    {
        private readonly IHostingEnvironment _env;
        private ApplicationStateService _appState;
        public HistoryController(IHostingEnvironment env, ApplicationStateService appState)
        {
            _env = env;
            _appState = appState;
        }
        public IActionResult Index()
        {
            var db = Database.GetDatabase(_env.ContentRootPath);
            var user = db.Users.FirstOrDefault();
            var ingredients = _appState.AllIngredients;
            var exercises = _appState.AllExercises;
            var dHistory = user.History.Where(h => h is DietHistory).Select(h => (DietHistory)h).OrderByDescending(h => h.EventDateTime).ToList();
            var fHistory = user.History.Where(h => h is FitnessHistory).Select(h => (FitnessHistory)h).OrderByDescending(h => h.EventDateTime).ToList();
            List<HistoryViewModelItem> vm = new List<HistoryViewModelItem>();
            fHistory.ForEach(f =>
            {
                TimeSpan time = default(TimeSpan);
                decimal amount = 0M;
                foreach ((Guid exerciseId, decimal amountTypeDependent, TimeSpan timeTaken) in f.RoutinePerformed.Exercises)
                {
                    time += timeTaken;
                    amount += amountTypeDependent * exercises.Single(e => e.ExerciseId == exerciseId).CaloriesPerUnit;
                }
                vm.Add(new HistoryViewModelItem()
                {
                    DateTime = f.EventDateTime,
                    Type = HistoryType.Fitness,
                    Calories = Math.Round(amount,1),
                    TimeSpent = time,
                    Name = f.RoutinePerformed.Name
                });
            });
            dHistory.ForEach(d =>
            {
                decimal amountResult = 0M;
                foreach ((Guid ingredientId, decimal amount) in d.Meal.IngredientsAndWeights)
                {
                    amountResult += amount * ingredients.Single(i => i.IngredientId == ingredientId).CaloriesPerGram;
                }
                vm.Add(new HistoryViewModelItem()
                {
                    DateTime = d.EventDateTime,
                    Type = HistoryType.Diet,
                    Name = d.Meal.Name,
                    Calories = Math.Round(amountResult,1)
                });
            });
            return View(vm.OrderByDescending(h => h.DateTime).ToList());
        }
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            ViewData["ScreenContext"] = ScreenContext.History;
            base.OnActionExecuting(context);
        }
    }
}