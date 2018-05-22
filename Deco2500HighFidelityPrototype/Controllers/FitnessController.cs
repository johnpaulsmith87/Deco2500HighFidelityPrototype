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
            var user = Database.GetDatabase(_env.ContentRootPath).Users.FirstOrDefault();
            var vm = new ExerciseIndexViewModel(user);
            //get user from database
            return View(vm);
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
                Exercise e = exercises.Single(ex => ex.ExerciseId == ex.ExerciseId);
                //get ingredient name
                Exercises.Add(e.Name);
                cal += e.CaloriesPerUnit * amountTypeDependent;
                TimeTaken += timeTaken;
            }
            Calories = cal;
            Date = fH.EventDateTime;
        }
        public decimal Calories { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan TimeTaken { get; set; }
        public List<string> Exercises { get; set; }
    }
}