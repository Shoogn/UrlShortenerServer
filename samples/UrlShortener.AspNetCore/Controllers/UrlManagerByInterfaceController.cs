using Microsoft.AspNetCore.Mvc;
using UrlShortener.Core;
using UrlShortener.EntityFramework.Store;

namespace UrlShortener.AspNetCore.Controllers
{
    public class UrlManagerByInterfaceController : Controller
    {
        private readonly IUrlShortenerManager _manager;
        public UrlManagerByInterfaceController(IUrlShortenerManager manager)
        {
            _manager = manager;
        }

        // GET:
        // UrlManagerByInterface/Index
        public async Task<IActionResult> Index()
        {
            string longUrl = "https://localhost:7031/UrlManagerByInterface/Data";
            var res = await _manager.CreateAsync(new ShortUrlEntity(), longUrl);
            return View();
        }

        //https://localhost:7031/sh/8VjVcQxhUIK
        public IActionResult Data()
        {
            return View();
        }
    }
}
