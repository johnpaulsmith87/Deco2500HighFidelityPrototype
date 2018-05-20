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
        public IActionResult Index(Guid? id)
        {
            
            var healthyIngredients = _appState.AllIngredients.Where(i => i.CaloriesPerGram < 3.4m); // use appState for displaying data to the frontend
            //example code for saving to database
            //always use _env.ContentRootPath -> points to wwwroot. the file path constants I use in 
            //dataaccess jump up one folder then look for the txts and json
            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
