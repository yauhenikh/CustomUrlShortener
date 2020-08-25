using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CustomUrlShortener.DataAccess.Entities;
using CustomUrlShortener.Services;
using CustomUrlShortener.WebApp.Models;

namespace CustomUrlShortener.WebApp.Controllers
{
    public class UrlController : Controller
    {
        private readonly IUrlShortenerService _urlShortenerService;

        public UrlController(IUrlShortenerService urlShortenerService)
        {
            _urlShortenerService = urlShortenerService;
        }

        public async Task<IActionResult> Index()
        {
            var urls = (await _urlShortenerService.GetAllAsync()).Select(urlInfo => new UrlViewModel
            {
                Id = urlInfo.Id,
                LongURL = urlInfo.LongUrl,
                URLToShow = urlInfo.LongUrl.Length > 40 ? $"{urlInfo.LongUrl.Substring(0, 40)}..." : urlInfo.LongUrl,
                ShortURL = $"{Request.Scheme}://{Request.Host}/{urlInfo.Token}",
                Created = urlInfo.Created,
                ClickNumber = urlInfo.ClickNumber
            });

            return View(urls);
        }

        public IActionResult Create()
        {
            UrlViewModel url = new UrlViewModel();
            return View(url);
        }

        [HttpPost]
        public async Task<IActionResult> Create(UrlViewModel url)
        {
            if (ModelState.IsValid)
            {
                UrlInfo urlInfo = await _urlShortenerService.ShortenUrlAsync(url.LongURL);
                url.ShortURL = $"{Request.Scheme}://{Request.Host}/{urlInfo.Token}";
            }

            return View(url);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            await _urlShortenerService.DeleteAsync(id.Value);

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Visit(string token)
        {
            string longUrl = await _urlShortenerService.IncreaseClicksAsync(token);

            return Redirect(longUrl);
        }
    }
}
