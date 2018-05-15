using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Deco2500HighFidelityPrototype.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Deco2500HighFidelityPrototype.Models.DataAccess;
using Deco2500HighFidelityPrototype.Models;

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
            return View();
        }
        //Fitness/GetFitnessGraphData/id?
        [HttpPost]
        public IEnumerable<FitnessHistory> GetFitnessGraphData(Guid? id)
        {
            var db = Database.GetDatabase(_env.ContentRootPath);
            var user = db.Users.SingleOrDefault(u => u.Id == id);
            return user.History.Where(h => h is FitnessHistory).Select(h => (FitnessHistory)h);
        }
    }
}