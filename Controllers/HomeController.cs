using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using TEST2.Models;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;

namespace TEST2.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }       
}
