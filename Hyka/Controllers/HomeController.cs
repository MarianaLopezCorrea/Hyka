using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Hyka.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Localization;

namespace Hyka.Controllers;

public class HomeController : Controller
{

    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }
    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    [HttpPost]
    public IActionResult LanguageHandler(string language, string returnUrl)
    {
        Response.Cookies.Append(CookieRequestCultureProvider.DefaultCookieName,
                                CookieRequestCultureProvider.MakeCookieValue(
                                    new RequestCulture(language)),
                                    new CookieOptions { Expires = DateTimeOffset.Now.AddDays(30) }
                                 );
        //return RedirectToAction(nameof(Index));
        return LocalRedirect(returnUrl);
    }
}
