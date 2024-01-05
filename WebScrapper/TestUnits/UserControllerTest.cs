using Microsoft.AspNetCore.Mvc;
using Moq;
using WebScrapperAPI.Repository;
using WebScrapperAPI.Controllers;
using WebScrapperAPI.Models;

public class UserControllerTests
{
    [Fact]
    public async Task Register_ValidModel_ReturnsOkResult()
    {
        var repositoryMock = new Mock<IWebScrapperRepository>();
        var configMock = new Mock<Microsoft.Extensions.Configuration.IConfiguration>();
        var userController = new UserController(repositoryMock.Object, configMock.Object);
        var userDTO = new UserDTO { UserName = "testuser", Email = "test@example.com", PasswordHash = "hashedpassword" };

        repositoryMock.Setup(repo => repo.CheckEmail(It.IsAny<string>())).ReturnsAsync(false);
        repositoryMock.Setup(repo => repo.CheckUserName(It.IsAny<string>())).ReturnsAsync(false);

        var result = await userController.Register(userDTO) as OkResult;

        Assert.NotNull(result);
        Assert.Equal(200, result.StatusCode);
    }

    [Fact]
    public async Task Register_EmailInUse_ReturnsBadRequest()
    {
        var repositoryMock = new Mock<IWebScrapperRepository>();
        var configMock = new Mock<Microsoft.Extensions.Configuration.IConfiguration>();
        var userController = new UserController(repositoryMock.Object, configMock.Object);
        var userDTO = new UserDTO { UserName = "testuser", Email = "test@example.com", PasswordHash = "hashedpassword" };

        repositoryMock.Setup(repo => repo.CheckEmail(It.IsAny<string>())).ReturnsAsync(true);

        var result = await userController.Register(userDTO) as BadRequestObjectResult;

        Assert.NotNull(result);
        Assert.Equal(400, result.StatusCode);
        Assert.Equal("Email is already in use", result.Value);
    }

    [Fact]
    public async Task Register_UserNameInUse_ReturnsBadRequest()
    {
        var repositoryMock = new Mock<IWebScrapperRepository>();
        var configMock = new Mock<Microsoft.Extensions.Configuration.IConfiguration>();
        var userController = new UserController(repositoryMock.Object, configMock.Object);
        var userDTO = new UserDTO { UserName = "testuser", Email = "test@example.com", PasswordHash = "hashedpassword" };

        repositoryMock.Setup(repo => repo.CheckEmail(It.IsAny<string>())).ReturnsAsync(false);
        repositoryMock.Setup(repo => repo.CheckUserName(It.IsAny<string>())).ReturnsAsync(true);

        var result = await userController.Register(userDTO) as BadRequestObjectResult;

        Assert.NotNull(result);
        Assert.Equal(400, result.StatusCode);
        Assert.Equal("UserName is already in use", result.Value);
    }


    [Fact]
    public async Task Login_ValidCredentials_ReturnsOkResultWithToken()
    {
        var repositoryMock = new Mock<IWebScrapperRepository>();
        var configMock = new Mock<Microsoft.Extensions.Configuration.IConfiguration>();
        var userController = new UserController(repositoryMock.Object, configMock.Object);
        var user = new User { Id = 1, UserName = "testuser", Email = "test@example.com", PasswordHash = "hashedpassword" };

        repositoryMock.Setup(repo => repo.Login(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(user);
        configMock.Setup(config => config["Jwt:SecretKey"]).Returns("i8h2VPuGjoEhVOLn3KCSoKhibKIMymU6eZrIU9XVeSs=");

        var result = await userController.Login("testuser", "hashedpassword") as OkObjectResult;

        Assert.NotNull(result);
        Assert.Equal(200, result.StatusCode);

        Assert.NotNull(result.Value.ToString());
    }

    [Fact]
    public async Task Login_InvalidCredentials_ReturnsUnauthorizedResult()
    {
        var repositoryMock = new Mock<IWebScrapperRepository>();
        var configMock = new Mock<Microsoft.Extensions.Configuration.IConfiguration>();
        var userController = new UserController(repositoryMock.Object, configMock.Object);

        repositoryMock.Setup(repo => repo.Login(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(new User()); // Empty user

        var result = await userController.Login("testuser", "invalidpassword") as UnauthorizedResult;

        Assert.NotNull(result);
        Assert.Equal(401, result.StatusCode);
    }

}
