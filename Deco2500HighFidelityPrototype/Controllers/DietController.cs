using System;
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
            ViewData["ScreenContext"] = ScreenContext.Diet | ScreenContext.CantGoBack;
            return View();
        }
        //Diet/GetDietGraphData/id?
        [HttpPost]
        public IEnumerable<DietHistoryGraphItem> GetDietGraphData(DietGraphReceiver data)
        {
            //works but we need to precalc meal stuff
            var db = Database.GetDatabase(_env.ContentRootPath);
            var user = db.Users.SingleOrDefault(u => u.Id == data.Id);
            var dietHistory = user.History.Where(h => h is DietHistory).Select(h => (DietHistory)h);
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