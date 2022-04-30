using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Hyka.Models;

namespace Hyka.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    // public IActionResult Index()
    // {
    //     return View();
    // }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    public IActionResult Index()
    {
        List<CarouselDataBinding> datasrc = new List<CarouselDataBinding>();
        Random rand = new Random(DateTime.Now.Millisecond);
        for (int i = 1; i < 4; i++)
        {
            long id = rand.NextInt64(100);

            datasrc.Add(new CarouselDataBinding
            {
                Id = i,
                ImgPath = $"https://picsum.photos/id/{id}/1920/420",
                Title = "Random" + i,
                Content = "Lorem ipsum dolor sit, amet consectetur adipisicing elit. Inventore eveniet sed saepe qui vel doloribus nam ipsum alias suscipit beatae velit totam, commodi iste dolores doloremque possimus pariatur nesciunt. Explicabo.",
                URL = $"https://picsum.photos/id/{id}/1920/1080"
            });
        }
        ViewBag.dataSource = datasrc;
        return View();
    }
    public class CarouselDataBinding
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string ImgPath { get; set; }
        public string URL { get; set; }
    }


}
