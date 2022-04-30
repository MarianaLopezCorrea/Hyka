using Hyka.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Hyka.Controllers
{
    public class AuthenticationController : Controller
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;

        public AuthenticationController(
            SignInManager<IdentityUser> signInManager,
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IConfiguration configuration

            )
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Login(LoginModel model)
        {
            // _signInManager.UserManager.
            var user = await _signInManager.UserManager.FindByNameAsync(model.Username);
            var result = await _signInManager.PasswordSignInAsync(user, model.Password, false, lockoutOnFailure: false);

            if (user == null || !result.Succeeded)
                ModelState.AddModelError("Username", "Invalid Username or Password");

            if (user != null && result.Succeeded)
            {
                var userRoles = await _userManager.GetRolesAsync(user);

                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };

                foreach (var userRole in userRoles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                }

                var token = GetToken(authClaims);
                TempData["success"] = "Logged in Sucessful";

                return Redirect("~/Decoder/Decode");
            }
            return View(Unauthorized());
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            var userExists = await _userManager.FindByNameAsync(model.Username);
            if (userExists != null)
                ModelState.AddModelError("Username", "Username already exists");

            if (ModelState.IsValid)
            {
                IdentityUser user = new()
                {
                    Email = model.Email,
                    SecurityStamp = Guid.NewGuid().ToString(),
                    UserName = model.Username
                };
                var result = await _userManager.CreateAsync(user, model.Password);

                if (!result.Succeeded)
                {
                    TempData["Error"] = "Error 505 - Error to create Blockbuster";
                    return BadRequest(View());
                }
                var blockbusterExists = !await _roleManager.RoleExistsAsync(Roles.Blockbuster);

                if (!blockbusterExists)
                    await _roleManager.CreateAsync(new IdentityRole(Roles.Blockbuster));

                await _userManager.AddToRoleAsync(user, Roles.Blockbuster);

                TempData["success"] = "Registered Sucessful";
                return RedirectToAction("Login");
            }
            return View(model);
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> RegisterAdmin(RegisterModel model)
        {
            var userExists = await _userManager.FindByNameAsync(model.Username);
            if (userExists != null)
                ModelState.AddModelError("Username", "Username already exists");

            if (ModelState.IsValid)
            {
                IdentityUser user = new()
                {
                    Email = model.Email,
                    SecurityStamp = Guid.NewGuid().ToString(),
                    UserName = model.Username
                };
                var result = await _userManager.CreateAsync(user, model.Password);

                if (!result.Succeeded)
                {
                    TempData["Error"] = "Error 505 ";
                    return BadRequest(View());
                }
                var adminRoleExists = await _roleManager.RoleExistsAsync(Roles.Admin);
                var blockbusterExists = !await _roleManager.RoleExistsAsync(Roles.Blockbuster);

                if (!adminRoleExists)
                    await _roleManager.CreateAsync(new IdentityRole(Roles.Admin));

                if (!blockbusterExists)
                    await _roleManager.CreateAsync(new IdentityRole(Roles.Blockbuster));

                await _userManager.AddToRoleAsync(user, Roles.Admin);
                await _userManager.AddToRoleAsync(user, Roles.Blockbuster);

                TempData["success"] = "Registered Sucessful";
                return RedirectToAction("Login");
            }
            return View(model);
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            TempData["success"] = "logged out successfully";
            return Redirect("~/Home/Index");
        }


        private JwtSecurityToken GetToken(List<Claim> authClaims)
        {
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                expires: DateTime.UtcNow.AddHours(12),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );

            return token;
        }
    }
}