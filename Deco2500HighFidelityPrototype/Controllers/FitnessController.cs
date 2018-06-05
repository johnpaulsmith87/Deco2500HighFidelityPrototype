using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Deco2500HighFidelityPrototype.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Deco2500HighFidelityPrototype.Models.DataAccess;
using Deco2500HighFidelityPrototype.Models;
using Deco2500HighFidelityPrototype.Models.ViewModels;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Deco2500HighFidelityPrototype.Controllers
{
    public class FitnessController : Controller
    {
        private readonly IHostingEnvironment _env;
        private ApplicationStateService _appState;
        public FitnessController(IHostingEnvironment env, ApplicationStateService appState)
        {
            _env = env;
            _appState = appState;
        }
        public IActionResult Index()
        {
            var db = Database.GetDatabase(_env.ContentRootPath);
            var user = db.Users[0];
            bool hasActiveRoutine = db.Users[0].History
                .OfType<FitnessHistory>()
                .Any(f => f.EventDateTime.Date == DateTime.Now.Date);
            var vm = new ExerciseIndexViewModel()
            {
                User = user,
                ActiveToday = hasActiveRoutine
            };
            if (hasActiveRoutine)
            {
                var today = db.Users[0].History
                .OfType<FitnessHistory>()
                .Where(fh => fh.EventDateTime.Date == DateTime.Now.Date)
                .OrderByDescending(fh => fh.EventDateTime)
                .FirstOrDefault();
                vm.Today = new FitnessHistoryGraphItem(today, _appState.AllExercises);
            }
            //get user from database
            return View(vm);
        }
        public IActionResult EditCurrentRoutine()
        {
            ViewData["ScreenContext"] = ScreenContext.Fitness | ScreenContext.EditCurrent | ScreenContext.CanGoBack;
            return View();
        }
        public IActionResult CreateRoutine()
        {
            ViewData["ScreenContext"] = ScreenContext.Fitness | ScreenContext.CreateRoutine | ScreenContext.CanGoBack;
            return View();
        }
        //Fitness/GetFitnessGraphData/id?
        [HttpPost]
        public IEnumerable<FitnessHistoryGraphItem> GetFitnessGraphData(FitnessGraphReceiver data)
        {
            //works but we need to precalc meal stuff
            var db = Database.GetDatabase(_env.ContentRootPath);
            var user = db.Users.SingleOrDefault(u => u.Id == data.Id);
            var fitnessHistory = user.History.Where(h => h is FitnessHistory).Select(h => (FitnessHistory)h);
            var exercises = _appState.AllExercises;
            // now we want to package this to be easy on the front end
            var result = fitnessHistory.Select(fh => new FitnessHistoryGraphItem(fh, exercises)).ToList();
            return result;
        }
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            ViewData["ScreenContext"] = ScreenContext.Fitness;
            base.OnActionExecuting(context);
        }
        public IEnumerable<ExerciseAutocompleteItem> GetAllExercises(ChooseExerciseReceiver data)
        {
            return _appState.AllExercises
                .Where(i => i.Name.StartsWith(data.Message, StringComparison.OrdinalIgnoreCase))
                .Select(i => new ExerciseAutocompleteItem()
                {
                    label = i.Name,
                    value = i.ExerciseId
                });
        }
    }
    //POCOs FOR AJAX - well some aren't simply POCOs
    public class ChooseExerciseReceiver
    {
        public string Message { get; set; }
    }
    public class ExerciseAutocompleteItem
    {
        public Guid value { get; set; }
        public string label { get; set; }
    }
    public class FitnessGraphReceiver
    {
        public Guid Id { get; set; }
    }
    public class FitnessHistoryGraphItem
    {
        public FitnessHistoryGraphItem(FitnessHistory fH, List<Exercise> exercises)
        {
            decimal cal = 0;
            Exercises = new List<string>();
            foreach (var (ExerciseId, amountTypeDependent, timeTaken) in fH.RoutinePerformed.Exercises)
            {
                Exercise e = exercises.Single(ex => ex.ExerciseId == ExerciseId);
                //get ingredient name
                Exercises.Add(e.Name);
                cal += e.CaloriesPerUnit * amountTypeDependent;
                TimeTaken += timeTaken;
            }
            Calories = cal;
            Date = fH.EventDateTime;
            Routine = fH.RoutinePerformed;
        }
        public decimal Calories { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan TimeTaken { get; set; }
        public List<string> Exercises { get; set; }
        public Routine Routine { get; set; }
    }
}