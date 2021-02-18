using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SimpleUrlShortener.Models;
using SimpleUrlShortener.Repositories;

namespace SimpleUrlShortener.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UrlRepository _urlRepository;

        public HomeController(ILogger<HomeController> logger, UrlRepository urlRepository)
        {
            _logger = logger;
            _urlRepository = urlRepository;
        }

        public IActionResult Index()
        {
            var keys = _urlRepository.GetAllKeys();
            return View(new IndexModel() { ExistingKeys = keys });
        }

        [HttpPost]
        public IActionResult AddUrl(AddUrlModel model)
        {
            var key = GenerateKey();
            _urlRepository.Insert(key, model.FullUrl);
            var keys = _urlRepository.GetAllKeys();

            return View("Index", new IndexModel() { ExistingKeys = keys });
        }

        [Route("{urlHash:regex(^\\w{{32}}$)}")]
        [HttpGet]
        public IActionResult RedirectTo(string urlHash)
        {
            var url = _urlRepository.Get(urlHash);
            if (string.IsNullOrEmpty(url))
                return NotFound();

            return Redirect(url);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        private string GenerateKey()
        {
            var result = Guid.NewGuid().ToString().Replace("-", "");

            return result;
        }
    }
}
