using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebScrapperAPI.Models;
using WebScrapperAPI.Repository;

namespace WebScrapperAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IWebScrapperRepository _repository;
        private readonly IConfiguration _config;

        public UserController(IWebScrapperRepository repository, IConfiguration config)
        {
            _repository = repository;
            _config = config;
        }

        [AllowAnonymous]
        [HttpPost("/register")]
        public async Task<IActionResult> Register([FromBody] UserDTO model)
        {
            if (await _repository.CheckEmail(model.UserName))
            {
                return BadRequest("Email is already in use");
            }

            if (await _repository.CheckUserName(model.UserName))
            {
                return BadRequest("UserName is already in use");
            }

            var newUser = new User()
            {
                UserName = model.UserName,
                Email = model.Email,
                PasswordHash = model.PasswordHash
            };

            await _repository.CreateUser(newUser);

            return Ok();
        }

        [AllowAnonymous]
        [HttpGet("/login")]
        public async Task<IActionResult> Login(string userName, string passwordHash)
        {
            var user = await _repository.Login(userName, passwordHash);

            if (string.IsNullOrEmpty(user.UserName))
            {
                return Unauthorized();
            }

            return Ok(GenerateJSONWebToken(user));
        }

        private string GenerateJSONWebToken(User userInfo)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:SecretKey"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[] {
                new Claim(JwtRegisteredClaimNames.Sub, userInfo.UserName),
                new Claim(JwtRegisteredClaimNames.Email, userInfo.Email),
                new Claim("UserId", userInfo.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(null,
              null,
              claims,
              expires: DateTime.Now.AddMinutes(120),
              signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
