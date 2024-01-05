using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebScrapperAPI.Models;
using WebScrapperAPI.Repository;

namespace WebScrapperAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WebScrapperController : ControllerBase
    {
        private readonly IWebScrapperRepository _repository;

        public WebScrapperController(IWebScrapperRepository repository) 
        {
            _repository = repository;
        }

        [Authorize]
        [HttpGet("/GetScrappedPages")]
        public async Task<IActionResult> GetScrappedPages()
        {
            var userId = Convert.ToInt32(HttpContext.User.Claims.FirstOrDefault(x => x.Type == "UserId").Value);

            return Ok(await _repository.GetPages(userId));
        }

        [Authorize]
        [HttpPost("/PostScrappePage")]
        public async Task<IActionResult> ScrappePage(ScrapperPageDTO model) 
        {
            var userId = Convert.ToInt32(HttpContext.User.Claims.FirstOrDefault(x => x.Type == "UserId").Value);

            var page = new ScrapedPage()
            {
                UserId = userId,
                PageName = model.PageName,
                Url = model.Url,
                LinkCount = 0,
                Links = new List<Link>()
            };

            await _repository.AddWebPage(page);

            return Ok();
        }
    }
}