using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Hyka.Areas.Identity.Data;
using Hyka.Data;
using Hyka.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace Hyka.Controllers
{
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("[controller]")]
    public class AuthenticationController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IConfiguration _config;

        public AuthenticationController(
            ApplicationDbContext db,
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            IConfiguration config)
        {
            _db = db;
            _userManager = userManager;
            _signInManager = signInManager;
            _config = config;
        }

        // GET endpoint to log in and get a token
        [AllowAnonymous]
        [HttpGet("getToken")]
        public async Task<IActionResult> GetToken([FromBody] LoginModel loginModel)
        {
            var user = _db.Users.FirstOrDefault(u => u.Email == loginModel.Email);

            if (user != null)
            {
                var signInResult = await _signInManager.CheckPasswordSignInAsync(user, loginModel.Password, false);

                if (signInResult.Succeeded)
                {
                    var tokenHandler = new JwtSecurityTokenHandler();
                    var key = Encoding.ASCII.GetBytes(_config.GetSection("Keys")["TokenSignIn"]);
                    var tokenDescriptor = new SecurityTokenDescriptor
                    {
                        Subject = new ClaimsIdentity(new Claim[]
                        {
                            new Claim(ClaimTypes.Name, loginModel.Email )
                        }),
                        Expires = DateTime.UtcNow.AddHours(1),
                        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                    };
                    var token = tokenHandler.CreateToken(tokenDescriptor);
                    var tokenString = tokenHandler.WriteToken(token);

                    return Ok(new { Token = tokenString });
                }
            }
            return BadRequest();
        }

        [HttpGet("getPersons")]
        public IActionResult Get()
        {
            return Ok(_db.People);
        }
    }
}