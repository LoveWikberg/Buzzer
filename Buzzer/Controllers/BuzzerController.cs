using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Buzzer.Controllers
{
    public class BuzzerController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
