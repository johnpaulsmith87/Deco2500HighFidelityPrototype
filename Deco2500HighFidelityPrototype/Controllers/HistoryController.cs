using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Deco2500HighFidelityPrototype.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Deco2500HighFidelityPrototype.Models;
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
            return View();
        }
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            ViewData["ScreenContext"] = ScreenContext.History;
            base.OnActionExecuting(context);
        }
    }
}