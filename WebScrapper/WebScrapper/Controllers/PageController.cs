using Microsoft.AspNetCore.Mvc;

namespace WebScrapper.Controllers
{
    public class PageController : Controller
    {
        private readonly IAPIConsumer _consumer;

        public PageController(IAPIConsumer consumer) 
        {
            _consumer = consumer;
        }

        public async Task<IActionResult> Index()
        {
            var token = HttpContext.Session.GetString("_token");
            var pages = await _consumer.GetPages(token);

            return View(pages);
        }
    }
}
