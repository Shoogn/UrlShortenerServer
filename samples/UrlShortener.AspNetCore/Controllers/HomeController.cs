using Microsoft.AspNetCore.Mvc;
using UrlShortener.Core;
using UrlShortener.EntityFramework.Store;

namespace UrlShortener.AspNetCore.Controllers
{
    public class HomeController : Controller
    {
        public readonly UrlShortenerManager<ShortUrlEntity> _manager;
        public HomeController(UrlShortenerManager<ShortUrlEntity> manager)
        {
            _manager = manager;
        }
        public async Task<IActionResult> Index()
        {
            string longUrl = "https://localhost:7031/Home/Data";
            var res = await _manager.CreateAsync(new ShortUrlEntity(), longUrl);
            return View();
        }


        public IActionResult Data()
        {
            return View();
        }
    }
}
