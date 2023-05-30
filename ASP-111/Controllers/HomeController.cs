using ASP_111.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using ASP_111.Services;

namespace ASP_111.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IDateService _dateService;

        public HomeController(ILogger<HomeController> logger, IDateService dateService)
        {
            _logger = logger;
            _dateService = dateService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult Intro()
        {
            return View();
        }

        public ViewResult Services()
        {
            ViewData["date"] = _dateService.GetDate();
            return View();
        }
        public ViewResult Data()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}