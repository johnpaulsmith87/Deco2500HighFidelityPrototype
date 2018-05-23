using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Deco2500HighFidelityPrototype.Models;
using Deco2500HighFidelityPrototype.Models.DataAccess;
using Microsoft.AspNetCore.Hosting;
using Deco2500HighFidelityPrototype.Services;
using Microsoft.AspNetCore.Mvc.Filters;
using Deco2500HighFidelityPrototype.Models.ViewModels;
namespace Deco2500HighFidelityPrototype.Controllers
{
    public class HomeController : Controller
    {
        private readonly IHostingEnvironment _env;
        private ApplicationStateService _appState;
        public HomeController(IHostingEnvironment env, ApplicationStateService appState)
        {
            _env = env;
            _appState = appState;
        }
        public IActionResult Index(Guid? id)
        {

            var db = Database.GetDatabase(_env.ContentRootPath);
            var user = db.Users.FirstOrDefault();
            var ingredients = _appState.AllIngredients;
            var dHistory = user.History.Where(h => h is DietHistory).Select(h => (DietHistory)h).OrderByDescending(h => h.EventDateTime).ToList();
            var fHistory = user.History.Where(h => h is FitnessHistory).Select(h => (FitnessHistory)h).OrderByDescending(h => h.EventDateTime).ToList();

            

            var goalTime = TimeSpan.Parse("01:00:00");
            var goalCalories = 2300M;
            TimeSpan fToday = default(TimeSpan);
            decimal dToday = 0M;
            var todayFitness = fHistory.Where(f => f.EventDateTime.Date == DateTime.Today).ToList();
            var todayDiet = dHistory.Where(d => d.EventDateTime.Date == DateTime.Today).ToList();

            foreach( var fitness in todayFitness)
            {
                foreach( (Guid exerciseId,decimal amountTypeDependent,TimeSpan timeTaken) in fitness.RoutinePerformed.Exercises)
                {
                    fToday += timeTaken;
                }
            }
            foreach (var diet in todayDiet)
            {
                foreach ((Guid ingredientId, decimal amount) in diet.Meal.IngredientsAndWeights)
                {
                    dToday += amount * ingredients.Single(i => i.IngredientId == ingredientId).CaloriesPerGram;
                }
            }
            var vm = new HomeIndexViewModel()
            {
                PercentOfDailyDietGoalCalories = Math.Min(Decimal.Divide(dToday, goalCalories),1.0M),
                PercentOfDailyWorkTime = Math.Min(Decimal.Divide(fToday.Ticks, goalTime.Ticks),1.0M),
                UserId = user.Id,
                Username = "Cornelius"
            };
            //example code for saving to database
            //always use _env.ContentRootPath -> points to wwwroot. the file path constants I use in 
            //dataaccess jump up one folder then look for the txts and json
            return View(vm);
        }


        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            ViewData["ScreenContext"] = ScreenContext.Home;
            base.OnActionExecuting(context);
        }
    }
}
