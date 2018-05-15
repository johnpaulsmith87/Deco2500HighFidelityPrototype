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
        public async Task<IActionResult> Index(Guid? id)
        {
            
            var healthyIngredients = _appState.AllIngredients.Where(i => i.CaloriesPerGram < 3.4m); // use appState for displaying data to the frontend
            //example code for saving to database
            //always use _env.ContentRootPath -> points to wwwroot. the file path constants I use in 
            //dataaccess jump up one folder then look for the txts and json
            if (false)
            {
                //this code is unreachable... just here as an example reading/writing to database
                var db = Database.GetDatabase(_env.ContentRootPath);
                var currentUser = db.Users.SingleOrDefault(user => user.Id == id);
                currentUser.Preferences.Privacy = !currentUser.Preferences.Privacy;

                await Database.SaveDatabase(db, _env.ContentRootPath);
            }
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
