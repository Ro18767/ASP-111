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
        public ViewResult Middleware()
        {
            ViewData["marker"] =
      HttpContext.Items.ContainsKey("marker")
      ? HttpContext.Items["marker"]
      : "Нет маркера";
            return View();
        }

        public IActionResult Razor()
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

        public ViewResult Sessions()
        {
            if (HttpContext.Session.Keys.Contains("StoredValue"))
            {
                ViewData["StoredValue"] = HttpContext.Session.GetString("StoredValue");
            }
            else
            {
                ViewData["StoredValue"] = "";
            }
            return View();
        }
        public RedirectToActionResult SetSession()
        {
            HttpContext.Session.SetString("StoredValue", "Данные в сессии");
            return RedirectToAction(nameof(Sessions));
            /* Browser                  ASP
             * 
             * SetSession -----------> redirect
             *  302       <----------     save in session
             *  location: Sessions                    |
             *                                        |
             * Sessions --------------> View          |
             *   200    <-------------    get from session
             */
        }
        public RedirectToActionResult RemoveSession()
        {
            HttpContext.Session.Remove("StoredValue");
            return RedirectToAction(nameof(Sessions));
        }
    }
}