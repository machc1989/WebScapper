using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WebScrapper.Models;

namespace WebScrapper.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IAPIConsumer _consumer;

        public HomeController(ILogger<HomeController> logger, IAPIConsumer consumer)
        {
            _logger = logger;
            _consumer = consumer;
        }

        public IActionResult Login()
        {
            return View("Login");
        }

        [HttpPost]
        public async Task<IActionResult> Login(Login model)
        {
            string token = await _consumer.Login(model.UserName, model.Password);

            if (string.IsNullOrEmpty(token))
            {
                return View();
            }

            this.HttpContext.Session.SetString("_token",token);

            return RedirectToAction("Index","Page");
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(UserModel model)
        {
            var success = await _consumer.CreateUser(model);

            if (success)
            {
                return Redirect("Login");
            }

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
