using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using WebScrapperAPI.Controllers;
using WebScrapperAPI.Models;
using WebScrapperAPI.Repository;

public class WebScrapperControllerTests
{
    [Fact]
    public async Task GetScrappedPages_ReturnsOkResultWithPages()
    {
        var repositoryMock = new Mock<IWebScrapperRepository>();
        var controller = new WebScrapperController(repositoryMock.Object);

        var userId = 1;
        var claims = new List<Claim> { new Claim("UserId", userId.ToString()) };
        var identity = new ClaimsIdentity(claims);
        var claimsPrincipal = new ClaimsPrincipal(identity);

        controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = claimsPrincipal }
        };

        var pages = new List<ScrapedPage>
        {
            new ScrapedPage { PageName = "Page 1", Url = "https://example.com/page1", LinkCount = 5 },
            new ScrapedPage { PageName = "Page 2", Url = "https://example.com/page2", LinkCount = 8 }
        };

        repositoryMock.Setup(repo => repo.GetPages(userId)).ReturnsAsync(pages);

        var result = await controller.GetScrappedPages() as OkObjectResult;

        Assert.NotNull(result);
        Assert.Equal(200, result.StatusCode);

        var returnedPages = result.Value as List<ScrapedPage>;
        Assert.NotNull(returnedPages);
        Assert.Equal(2, returnedPages.Count);
    }

    [Fact]
    public async Task ScrappePage_ReturnsOkResult()
    {
        var repositoryMock = new Mock<IWebScrapperRepository>();
        var controller = new WebScrapperController(repositoryMock.Object);

        var userId = 1; 
        var claims = new List<Claim> { new Claim("UserId", userId.ToString()) };
        var identity = new ClaimsIdentity(claims);
        var claimsPrincipal = new ClaimsPrincipal(identity);

        controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = claimsPrincipal }
        };

        var model = new ScrapperPageDTO { PageName = "Test Page", Url = "https://example.com/test" };

        var result = await controller.ScrappePage(model) as OkResult;

        Assert.NotNull(result);
        Assert.Equal(200, result.StatusCode);

        repositoryMock.Verify(repo => repo.AddWebPage(It.Is<ScrapedPage>(
            page => page.UserId == userId && page.PageName == model.PageName && page.Url == model.Url)), Times.Once);
    }
}
