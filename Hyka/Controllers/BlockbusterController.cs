using Hyka.Areas.Identity.RolesDefinition;
using Hyka.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Identity.UI.Services;
using System.Text.Json;
using Hyka.Dtos;

namespace Hyka.Controllers
{
    [Authorize(Roles = $"{Roles.ADMIN}")]
    [AutoValidateAntiforgeryToken]
    public class BlockbusterController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IUserStore<IdentityUser> _userStore;
        private readonly IUserEmailStore<IdentityUser> _emailStore;
        private readonly IEmailSender _emailSender;
        private readonly ILogger<Blockbuster> _logger;

        public BlockbusterController(
            UserManager<IdentityUser> userManager,
            IUserStore<IdentityUser> userStore,
            IEmailSender emailSender,
            ILogger<Blockbuster> logger)
        {
            _emailSender = emailSender;
            _userManager = userManager;
            _userStore = userStore;
            _emailStore = _getEmailStore();
            _logger = logger;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync(Blockbuster blockbuster)
        {
            if (ModelState.IsValid)
            {
                var user = _createUser();
                await _userStore.SetUserNameAsync(user, blockbuster.UserName, CancellationToken.None);
                await _emailStore.SetEmailAsync(user, blockbuster.Email, CancellationToken.None);
                var bytes = RandomNumberGenerator.GetBytes(128 / 8);
                var randomPassword = Convert.ToBase64String(bytes);
                var result = await _userManager.CreateAsync(user, randomPassword);
                if (result.Succeeded)
                {
                    var currentAdmin = await _userManager.GetUserAsync(User);
                    await _userManager.AddToRoleAsync(user, Roles.BLOCKBUSTER);
                    _logger.LogInformation($"{User.Identity.Name} has registered a new user {user.UserName}");
                    var loginUrl = Url.Page("/Account/Login", values: new { area = "Identity" });
                    var message = new MessageDto
                    {
                        Head = $"Hola, {user.UserName}. Bienvenid@ al PAF!",
                        Body = $"Ahora eres taquillero." +
                               $"Tus credenciales de acceso:\n" +
                               $"Usuario: {user.UserName}\n" +
                               $"Correo: {user.Email}\n" +
                               $"Contraseña: {randomPassword}\n\n" +
                               $"Clic en el boton para logearte",
                        Url = loginUrl
                    };
                    await _emailSender.SendEmailAsync(
                        user.Email,
                        $"Cuenta para taquilla PAF creada",
                        JsonSerializer.Serialize(message)
                    );
                    TempData["status"] = JsonSerializer.Serialize(new { type = "success", message = "Se ha notificado al correo." });
                    return RedirectToAction("Index");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return View("Index", blockbuster);
        }

        private IdentityUser _createUser()
        {
            try
            {
                return Activator.CreateInstance<IdentityUser>();
            }
            catch
            {
                throw new InvalidOperationException($"Can't create an instance of '{nameof(IdentityUser)}'. " +
                    $"Ensure that '{nameof(IdentityUser)}' is not an abstract class and has a parameterless constructor, or alternatively " +
                    $"override the register page in /Areas/Identity/Pages/Account/Register.cshtml");
            }
        }

        private IUserEmailStore<IdentityUser> _getEmailStore()
        {
            if (!_userManager.SupportsUserEmail)
            {
                throw new NotSupportedException("The default UI requires a user store with email support.");
            }
            return (IUserEmailStore<IdentityUser>)_userStore;
        }

    }
}
