using Microsoft.AspNetCore.Mvc;
using Movies_Point.Models;
using Movies_Point.Data;
using System.Diagnostics;
using Movies_Point.IRepository;

namespace Movies_Point.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        IMovieRepository movieRepository;

        public HomeController(ILogger<HomeController> logger, IMovieRepository movieRepository)
        {
            _logger = logger;
            this.movieRepository = movieRepository;
        }

        public IActionResult Index()
        {
            var movies = movieRepository.ReadAll();
            return View(movies);
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
    }
}
