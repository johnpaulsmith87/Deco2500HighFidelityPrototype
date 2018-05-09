using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace Deco2500HighFidelityPrototype.Controllers
{
    public class DietController : Controller
    {
        private readonly IHostingEnvironment _env;
        public DietController(IHostingEnvironment env)
        {
            _env = env;
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}