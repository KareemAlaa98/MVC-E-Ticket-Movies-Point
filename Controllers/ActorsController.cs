using Microsoft.AspNetCore.Mvc;
using Movies_Point.Data;
using Movies_Point.Models;
using Movies_Point.IRepository;
using Movies_Point.Repository;
using Microsoft.AspNetCore.Authorization;
using Movies_Point.ViewModels;

namespace Movies_Point.Controllers
{
    public class ActorsController : Controller
    {
        IActorRepository actorRepository;
        IMovieRepository movieRepository;
        public ActorsController(IActorRepository actorRepository, IMovieRepository movieRepository)
        {
            this.actorRepository = actorRepository;
            this.movieRepository = movieRepository;
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult Index()
        {
            var actors = actorRepository.ReadAll();
            return View(actors);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            ViewData["listOfMovies"] = movieRepository.ReadAll().Select(e => new { Id = e.Id, Name = e.Name });
            ActorViewModel actorVM = new ActorViewModel();
            return View(actorVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public IActionResult SaveNew(ActorViewModel actorVM)
        {
            if (ModelState.IsValid)
            {
                actorRepository.Create(actorVM);
                return RedirectToAction("Index");
            }
            ViewData["listOfMovies"] = movieRepository.ReadAll().Select(e => new { Id = e.Id, Name = e.Name });
            return View("Create", actorVM);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult Edit(int id) 
        {
            var actor = actorRepository.ReadById(id);
            if(actor == null) { RedirectToAction("Index"); }
            ActorViewModel actorVM = new ActorViewModel();
            actorVM.Id = actor.Id;
            actorVM.FirstName = actor.FirstName;
            actorVM.LastName = actor.LastName;
            actorVM.Bio = actor.Bio;
            actorVM.ProfilePicture = actor.ProfilePicture;
            actorVM.News = actor.News;

            actorRepository.GetAlreadySelectedMovies(actorVM);

            ViewData["listOfMovies"] = movieRepository.ReadAll().Select(e => new { Id = e.Id, Name = e.Name });
            return View(actorVM); 
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public IActionResult SaveEdit(ActorViewModel actorVM) 
        {
            if (ModelState.IsValid)
            {
                actorRepository.Update(actorVM);
                return RedirectToAction("Index");
            }

            ViewData["listOfMovies"] = movieRepository.ReadAll().Select(e => new { Id = e.Id, Name = e.Name });
            return View("Edit", actorVM);

        }
        

        [Authorize(Roles ="Admin")]
        public IActionResult Delete(int id)
        {
            if (actorRepository.Delete(id))
            {
                return RedirectToAction("Index");
            };
            return View();
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult Details(int id)
        {
            var actor = actorRepository.ReadById(id);
            return View(actor);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Movies(int id)
        {
            //var actorMovies = from movie in context.Movies
            //                  join actorMovie in context.ActorMovies on movie.Id equals actorMovie.MovieId
            //                  join actor in context.Actors on actorMovie.ActorId equals actor.Id
            //                  where actor.Id == id
            //                  select movie;

            var actor = actorRepository.ReadById(id);
            ViewData["actorFirstName"] = actor.FirstName;
            ViewData["actorLastName"] = actor.LastName;
            var actorMovies = actorRepository.GetActorMovies(id);
            return View(actorMovies);
        }
    }
}
