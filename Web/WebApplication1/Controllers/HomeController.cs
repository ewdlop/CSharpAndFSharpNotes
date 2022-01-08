using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly BlobServiceClient _blobServiceClient;
        private readonly HttpClient _httpClient;
        public HomeController(ILogger<HomeController> logger,
            BlobServiceClient blobServiceClient,
            HttpClient httpClient)
        {
            _logger = logger;
            _blobServiceClient = blobServiceClient;
            _httpClient = httpClient;
        }

        public IActionResult Index()
        {
            Console.WriteLine(_httpClient.GetHashCode());
            return View();
        }

        public IActionResult Privacy()
        {
            Console.WriteLine(_httpClient.GetHashCode());
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}