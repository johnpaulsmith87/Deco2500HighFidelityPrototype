﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Deco2500HighFidelityPrototype.Services;
using Deco2500HighFidelityPrototype.Models.DataAccess;
using Deco2500HighFidelityPrototype.Models;
using Deco2500HighFidelityPrototype.Models.ViewModels;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Deco2500HighFidelityPrototype.Controllers
{
    public class DietController : Controller
    {
        private readonly IHostingEnvironment _env;
        private ApplicationStateService _appState;
        public DietController(IHostingEnvironment env, ApplicationStateService appState)
        {
            _env = env;
            _appState = appState;
        }
        public IActionResult Index()
        {
            var user = Database.GetDatabase(_env.ContentRootPath).Users.FirstOrDefault();
            var vm = new DietIndexViewModel(user);
            //get user from database
            return View(vm);
        }
        public IActionResult AddMeal()
        {
            ViewData["ScreenContext"] = ScreenContext.Diet | ScreenContext.CanGoBack | ScreenContext.AddMeal;
            return View();
        }
        public IActionResult ChooseMeal()
        {
            var db = Database.GetDatabase(_env.ContentRootPath);
            var ingredients = _appState.AllIngredients;
            var user = db.Users.FirstOrDefault();
            var last3Meals = user.History.AsParallel()
                .Where(h => h is DietHistory)
                .Select(h => new DietHistoryGraphItem((DietHistory)h, ingredients))
                .TakeLast(3).ToList();
            var steak = ingredients.Where(i => i.Name == "steak").FirstOrDefault();
            var asparagus = ingredients.Where(i => i.Name == "aspargus").FirstOrDefault();
            var bacon = ingredients.Where(i => i.Name == "bacon").FirstOrDefault();
            var haddock = ingredients.Where(i => i.Name == "haddock").FirstOrDefault();
            var spinach = ingredients.Where(i => i.Name == "spinach").FirstOrDefault();
            var suggestedMeal1 = new Meal()
            {
                MealId = Guid.NewGuid(),
                Name = "Steak and asparagus",
                IngredientsAndWeights = new List<(Guid IngredientId, decimal weightInGrams)>()
                {
                    (steak.IngredientId, 300M),
                    (asparagus.IngredientId, 150M)
                }
            };
            var suggestedMeal2 = new Meal()
            {
                MealId = Guid.NewGuid(),
                Name = "Haddock, bacon and spinach",
                IngredientsAndWeights = new List<(Guid IngredientId, decimal weightInGrams)>()
                {
                    (haddock.IngredientId, 200M),
                    (bacon.IngredientId, 100M),
                    (spinach.IngredientId, 100M)
                }
            };
            var vm = new ChooseMealViewModel()
            {
                HistoricalMeals = last3Meals,
                SuggestedMeals = new List<DietHistoryGraphItem>()
                {
                    new DietHistoryGraphItem(new DietHistory(){ Meal = suggestedMeal1 }, ingredients),
                    new DietHistoryGraphItem(new DietHistory() { Meal = suggestedMeal2 }, ingredients)
                }
            };
            ViewData["ScreenContext"] = ScreenContext.Diet | ScreenContext.CanGoBack | ScreenContext.ChooseMeal;
            return View(vm);
        }
        public IActionResult CreateMeal()
        {
            ViewData["ScreenContext"] = ScreenContext.Diet | ScreenContext.CanGoBack | ScreenContext.CreateMeal;
            return View();
        }
        //Diet/GetDietGraphData/id?
        [HttpPost]
        public IEnumerable<DietHistoryGraphItem> GetDietGraphData(DietGraphReceiver data)
        {
            //works but we need to precalc meal stuff
            var db = Database.GetDatabase(_env.ContentRootPath);
            var user = db.Users.SingleOrDefault(u => u.Id == data.Id);
            var dietHistory = user.History
                .Where(h => h is DietHistory)
                .Select(h => (DietHistory)h)
                .Take(5)
                .ToList();
            var ingredients = _appState.AllIngredients;
            // now we want to package this to be easy on the front end
            var result = dietHistory.Select(dh => new DietHistoryGraphItem(dh, ingredients)).ToList();
            return result;
        }
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            ViewData["ScreenContext"] = ScreenContext.Diet;
            base.OnActionExecuting(context);
        }
    }
    //POCOs for AJAX
    public class DietGraphReceiver
    {
        public Guid Id { get; set; }
    }
    public class DietHistoryGraphItem
    {
        public DietHistoryGraphItem(DietHistory dH, List<Ingredient> ingredients)
        {
            decimal cal = 0;
            Ingredients = new List<string>();
            foreach (var (IngredientId, weightInGrams) in dH.Meal.IngredientsAndWeights)
            {
                Ingredient i = ingredients.Single(ing => ing.IngredientId == IngredientId);
                //get ingredient name
                Ingredients.Add(i.Name);
                cal += i.CaloriesPerGram * weightInGrams;
            }
            Calories = cal;
            Date = dH.EventDateTime;
        }
        public decimal Calories { get; set; }
        public DateTime Date { get; set; }
        public List<string> Ingredients { get; set; }
    }
}