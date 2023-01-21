// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.AspNetCore.Identity.UI.Services;
using Hyka.Areas.Identity.RolesDefinition;
using Hyka.Dtos;
using System.Text.Json;
using System.Text.Encodings.Web;

namespace Hyka.Areas.Identity.Pages.Account
{
    public class ConfirmAdminModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IEmailSender _emailSender;

        public ConfirmAdminModel(
            UserManager<IdentityUser> userManager,
            IEmailSender emailSender
        )
        {
            _userManager = userManager;
            _emailSender = emailSender;
        }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [TempData]
        public string StatusMessage { get; set; }
        public async Task<IActionResult> OnGetAsync(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return RedirectToPage("/Index");
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{userId}'.");
            }

            code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
            var result = await _userManager.VerifyUserTokenAsync(user, TokenOptions.DefaultProvider, "AdminRequest", code);
            var userName = await _userManager.GetUserNameAsync(user);
            var email = await _userManager.GetEmailAsync(user);
            if (result)
            {
                var roles = await _userManager.GetRolesAsync(user);
                await _userManager.RemoveFromRolesAsync(user, roles);
                await _userManager.AddToRoleAsync(user, Roles.ADMIN);
                var loginUrl = Url.Page("/Account/Login", values: new { area = "Identity" });
                await _emailSender.SendEmailAsync(email, "Petición aprobada",
                JsonSerializer.Serialize(new MessageDto()
                {
                    Head = $"Hola, {userName}",
                    Body = $"Tu petición ha sido aprobada. Ahora eres administrador.",
                    Url = HtmlEncoder.Default.Encode(loginUrl)
                }));
                StatusMessage = $"Se ha enviado un mensaje de confirmación a {email}.";
                return Page();
            }
            StatusMessage = "Error al confirmar la petición.";
            return RedirectToPage("/Index");
        }
    }
}
