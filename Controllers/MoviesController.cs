using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc;
using Movies_Point.IRepository;
using Movies_Point.Models;
using Movies_Point.ViewModels;
using System.Net.WebSockets;
using Microsoft.AspNetCore.Authorization;
namespace Movies_Point.Controllers
{
    public class MoviesController : Controller
    {
        IMovieRepository movieRepository;
        ICategoryRepository categoryRepository;
        ICinemaRepository cinemaRepository;
        IActorRepository actorRepository;
        IEmailSenderRepository emailSender;
        public MoviesController(IActorRepository _actorRepository, IMovieRepository _movieRepository, ICategoryRepository _categoryRepository, ICinemaRepository _cinemaRepository, IEmailSenderRepository _emailSender)
        {
            this.movieRepository = _movieRepository;
            this.categoryRepository = _categoryRepository;
            this.cinemaRepository = _cinemaRepository;
            this.emailSender = _emailSender;
            this.actorRepository = _actorRepository;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Index()
        {
            var movies = movieRepository.ReadAll();
            return View(movies);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            ViewData["listofCategories"] = categoryRepository.ReadAll();
            ViewData["listofCinemas"] = cinemaRepository.ReadAll().Select(e => new { Id = e.Id, Name = e.Name });
            ViewData["listOfActors"] = actorRepository.ReadAll().Select(e=>new {Id = e.Id, FullName = $"{e.FirstName} {e.LastName}" });
            var movieVM = new MoviesViewModel();
            return View(movieVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public IActionResult SaveNew(MoviesViewModel movieVM)
        {
            if (ModelState.IsValid)
            {
                movieRepository.Create(movieVM);
                return RedirectToAction("Index");
            }

            ViewData["listofCategories"] = categoryRepository.ReadAll();
            ViewData["listofCinemas"] = cinemaRepository.ReadAll().Select(e => new { Id = e.Id, Name = e.Name });
            ViewData["listOfActors"] = actorRepository.ReadAll().Select(e=>new {Id = e.Id, FullName = $"{e.FirstName} {e.LastName}" });
            return View("create", movieVM);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult Edit(int id)
        {
            var movie = movieRepository.ReadById(id);
            if (movie == null) { return RedirectToAction("Index"); }

            var movieVM = new MoviesViewModel();
            movieVM.Id = movie.Id;
            movieVM.Name = movie.Name;
            movieVM.Price = movie.Price;
            movieVM.Description = movie.Description;
            movieVM.ImgUrl = movie.ImgUrl;
            movieVM.TrailerUrl = movie.TrailerUrl;
            movieVM.CategoryId = movie.CategoryId;
            movieVM.CinemaId = movie.CinemaId;
            movieVM.StartDate = movie.StartDate;
            movieVM.EndDate = movie.EndDate;

            movieRepository.GetAlreadySelectedActors(movieVM);

            ViewData["listofCategories"] = categoryRepository.ReadAll();
            ViewData["listofCinemas"] = cinemaRepository.ReadAll().Select(e => new { Id = e.Id, Name = e.Name });
            ViewData["listOfActors"] = actorRepository.ReadAll().Select(e => new { Id = e.Id, FullName = $"{e.FirstName} {e.LastName}" });
            return View(movieVM);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public IActionResult SaveEdit(MoviesViewModel movieVM)
        {
            if (ModelState.IsValid)
            {
                movieRepository.Update(movieVM);
                return RedirectToAction("Index");
            }
            ViewData["listofCategories"] = categoryRepository.ReadAll();
            ViewData["listofCinemas"] = cinemaRepository.ReadAll().Select(e => new { Id = e.Id, Name = e.Name });
            ViewData["listOfActors"] = actorRepository.ReadAll().Select(e => new { Id = e.Id, FullName = $"{e.FirstName} {e.LastName}" });
            return View("Edit", movieVM);
        }


        [Authorize(Roles = "Admin")]
        public IActionResult Delete(int id)
        {
            movieRepository.Delete(id);
            return RedirectToAction("Index");
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Details(int id)
        {
            Movie? movieReq = movieRepository.ReadById(id);

            var movieCast = movieRepository.GetMovieCast(id);

            ViewData["actors"] = movieCast;

            movieRepository.UpdateViewCount(id);

            return View(movieReq);
        }

        [Authorize(Roles = "User")]
        
        // Action to add movie to cart
        public IActionResult AddToCart(int id)
        {
            var movie = movieRepository.ReadById2(id);

            // Retrieve existing cart items from session
            List<Movie> movies = new List<Movie>();
            string? moviesAsString = HttpContext.Session.GetString("StringMovies");
            if (!string.IsNullOrEmpty(moviesAsString))
            {
                movies = JsonConvert.DeserializeObject<List<Movie>>(moviesAsString);
            }

            // Check if the movie already exists in the cart
            if (movie != null && !movies.Any(m => m.Id == movie.Id))
            {
                movies.Add(movie);

                // Serialize the updated list and store it back in the session
                string updatedMoviesString = JsonConvert.SerializeObject(movies);
                HttpContext.Session.SetString("StringMovies", updatedMoviesString);
            }

            // Redirect to the cart action
            return RedirectToAction("Cart");
        }


        [Authorize(Roles = "User")]
        // Action to display cart
        public IActionResult Cart()
        {
            // Retrieve cart items from session
            List<Movie> movies = new List<Movie>();
            string? moviesAsString = HttpContext.Session.GetString("StringMovies");
            if (!string.IsNullOrEmpty(moviesAsString))
            {
                movies = JsonConvert.DeserializeObject<List<Movie>>(moviesAsString);
            }

            return View(movies);
        }

        [Authorize]
        public async Task<IActionResult> SendEmail()
        {
            List<Movie> movies = new List<Movie>();
            string? moviesAsString = HttpContext.Session.GetString("StringMovies");
            string movieNames = "";
            var total = 0.0;
            DateTime? ticketDate;

            if (!string.IsNullOrEmpty(moviesAsString))
            {
                movies = JsonConvert.DeserializeObject<List<Movie>>(moviesAsString);
            }

            for(var i =0; i < movies.Count; i++)
            {
                total += movies[i].Price;
                movieNames += $"{movies[i].Name}, ";
            }

            var timeDifference = (movies[0].StartDate - DateTime.Now).TotalHours;

            if (timeDifference >= 5) 
            {
                ticketDate = movies[0].StartDate;
            }
            else { ticketDate = movies[0].StartDate.AddDays(1); }

            var reciever = "usermoviespoint@gmail.com";
            var subject = "Ticket Reservation";
            var message = $"Thank You for choosing Movies Point. You got {movies.Count} {(movies.Count > 1 ? "tickets" : "ticket")} to {movieNames} for ${(float)total}. Your movie starts at {ticketDate}.";

            // Clear the cart after confirming
            HttpContext.Session.Remove("StringMovies");

            await emailSender.SendEmailAsync(reciever, subject, message);

            return View();
        }

        [Authorize]
        public IActionResult RemoveFromCart(int id)
        {
            // Retrieve cart items from session
            List<Movie> movies = new List<Movie>();
            string? moviesAsString = HttpContext.Session.GetString("StringMovies");
            if (!string.IsNullOrEmpty(moviesAsString))
            {
                movies = JsonConvert.DeserializeObject<List<Movie>>(moviesAsString);
            }

            // Find and remove the movie with the specified ID from the cart
            var movieToRemove = movies.FirstOrDefault(m => m.Id == id);
            if (movieToRemove != null)
            {
                movies.Remove(movieToRemove);

                // Serialize the updated list and store it back in the session
                string updatedMoviesString = JsonConvert.SerializeObject(movies);
                HttpContext.Session.SetString("StringMovies", updatedMoviesString);
            }

            // Redirect back to the cart action
            return RedirectToAction("Cart");
        }

    }
}
