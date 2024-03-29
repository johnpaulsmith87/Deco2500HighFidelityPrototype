﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Deco2500HighFidelityPrototype.Models.DataAccess;
using Deco2500HighFidelityPrototype.Models;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Deco2500HighFidelityPrototype.Controllers
{
    public class SettingsController : Controller
    {
        private readonly IHostingEnvironment _env;
        public SettingsController(IHostingEnvironment env)
        {
            _env = env;
        }
        public IActionResult Index()
        {
            return View();
        }
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            ViewData["ScreenContext"] = ScreenContext.Settings;
            base.OnActionExecuting(context);
        }
    }
}