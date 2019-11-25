using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using static Microsoft.AspNetCore.Http.StatusCodes;

namespace ArcaneApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository repo;
        private readonly AppSettings appSettings;

        public UserController(IUserRepository repository, IOptions<AppSettings> appSettings) {
            repo = repository;
            this.appSettings = appSettings.Value;
        }

        [HttpGet("{userId}")]
        public ActionResult Get(int userId)
        {
            if(userId <= 0) {
                return BadRequest();
            }

            var result = repo.Get(userId);
            if(result == null) {
                return NotFound(userId);
            }

            return Ok(result);
        }

        [AllowAnonymous]
        [ValidateModelAttribute]
        [HttpPost("register")]
        public ActionResult Register([FromBody] User user)
        {
            var loggedUser = repo.Add(user);
            return Ok(loggedUser);
        }

        [AllowAnonymous]
        [ValidateModelAttribute]
        [HttpPost("authenticate")]
        public ActionResult Authenticate([FromBody] User user)
        {
            var loggedUser = repo.Authenticate(user);
            if(loggedUser == null) {
                return BadRequest("Username or password incorrect.");
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[] 
                {
                    new Claim(ClaimTypes.Name, user.Id.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            return Ok(new {
                Token = tokenString,
                User = loggedUser
            });
        }
    }
}
