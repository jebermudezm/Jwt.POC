using Jwt.POC.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Jwt.POC.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        public readonly AppSettings _appSettings;
        public AuthController(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
        }

        [HttpPost]
        public async Task<IActionResult> Authenticate(Login login)
        {
            if (login.UserName == "test" && login.Password == "123")
            {
                var user = new User 
                { 
                    Id = 1,
                    UserName = login.UserName,
                    Password = login.Password,
                    Name = "test"
                };
                user.Token = await BuildToken(user);
                return Ok(user);
            }
            if (login.UserName.Length == 0 || login.Password.Length == 0)
            {
                return NotFound("User Name and Password mus have value!!!");
            }
            return BadRequest("Login failed!!!");
        }

        private async Task<string> BuildToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var minutesToExpire = int.Parse(_appSettings.MinutesToExpire);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.UserName)
                }),
                Expires = DateTime.UtcNow.AddMinutes(minutesToExpire),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Issuer = _appSettings.Issuer,
                Audience = _appSettings.Audience,

            };
            var token = tokenHandler.WriteToken(tokenHandler.CreateToken(tokenDescriptor));
            return token;
        }
    }
}
