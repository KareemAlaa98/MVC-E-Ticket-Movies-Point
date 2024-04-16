using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Movies_Point.Data;
using Movies_Point.IRepository;
using Movies_Point.Models;
using Movies_Point.Repository;
using Movies_Point.ViewModels;
namespace Movies_Point.Controllers
{
    public class CinemasController : Controller
    {
        ICinemaRepository cinemaRepository;
        IMovieRepository movieRepository;
        public CinemasController(ICinemaRepository cinemaRepository, IMovieRepository movieRepository)
        {
            this.cinemaRepository = cinemaRepository;
            this.movieRepository = movieRepository;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Index()
        {
            List<Cinema> cinemas = cinemaRepository.ReadAll();
            return View(cinemas);
        }

        [HttpGet]
        [Authorize(Roles ="Admin")]
        public IActionResult Create()
        {
            ViewData["listOfMovies"] = movieRepository.ReadAll().Select(e => new { Id = e.Id, Name = e.Name });
            CinemaViewModel cinemaVM = new CinemaViewModel();
            return View(cinemaVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles ="Admin")]
        public IActionResult SaveNew(CinemaViewModel cinemaVM)
        {
            if (ModelState.IsValid)
            {
                cinemaRepository.Create(cinemaVM);
                return RedirectToAction("Index");
            }
            
            ViewData["listOfMovies"] = movieRepository.ReadAll().Select(e => new { Id = e.Id, Name = e.Name });
            return View("Create", cinemaVM);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            Cinema cinema = cinemaRepository.ReadById(id);
            if(cinema == null) { RedirectToAction("Index"); }
            CinemaViewModel cinemaVM = new CinemaViewModel();
            cinemaVM.Id = cinema.Id;
            cinemaVM.Name = cinema.Name;
            cinemaVM.Address = cinema.Address;
            //cinemaVM.CinemaLogo = cinema.CinemaLogo;
            cinemaVM.Description = cinema.Description;

            cinemaRepository.GetAlreadySelectedMovies(cinemaVM);
            ViewData["listOfMovies"] = movieRepository.ReadAll().Select(e => new { Id = e.Id, Name = e.Name });

            return View(cinemaVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles ="Admin")]
        public IActionResult SaveEdit(CinemaViewModel cinemaVM)
        {
            if (ModelState.IsValid)
            {
                cinemaRepository.Update(cinemaVM);
                return RedirectToAction("Index");
            }
            ViewData["listOfMovies"] = movieRepository.ReadAll().Select(e => new { Id = e.Id, Name = e.Name });

            return View("Edit", cinemaVM);
        }

        [Authorize(Roles ="Admin")]
        public IActionResult Delete(int id)
        {
            if (cinemaRepository.Delete(id))
            {
                return RedirectToAction("Index");
            };
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Details(int id)
        {
            //var cinemaMovies = context.Movies.Include("Category").Include("Cinema").Where(e => e.CinemaId == id).ToList();
            var cinemaMovies = movieRepository.ReadWithCategAndCinema(id);
            ViewData["cinemaName"] = cinemaRepository.ReadById(id).Name;

            return View(cinemaMovies);
        }
    }
}
