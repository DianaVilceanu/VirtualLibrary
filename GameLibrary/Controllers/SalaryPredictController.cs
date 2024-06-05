using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace GameLibrary.Controllers
{
    public class SalaryPredictController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}