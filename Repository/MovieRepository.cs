using Microsoft.EntityFrameworkCore;
using Movies_Point.Data;
using Movies_Point.IRepository;
using Movies_Point.Models;
using Movies_Point.ViewModels;

namespace Movies_Point.Repository
{
    public class MovieRepository : IMovieRepository
    {
        ApplicationDbContext context;
        public MovieRepository(ApplicationDbContext context)
        {
            this.context = context;
        }
        public List<Movie> ReadAll()
        {
            return context.Movies.Include("Category").Include("Cinema").ToList();
        }

        public Movie ReadById(int id)
        {
            var movie = context.Movies.Include("Category").Include("Cinema").FirstOrDefault(e => e.Id == id);
            return movie;            
        }
        public Movie ReadById2(int id)
        {
            var movie = context.Movies.FirstOrDefault(e => e.Id == id);
            return movie;            
        }

        public void Create(MoviesViewModel movieVM)
        {
            var newMovie = new Movie
            {
                Name = movieVM.Name,
                Price = movieVM.Price,
                Description = movieVM.Description,
                CinemaId = movieVM.CinemaId,
                CategoryId = movieVM.CategoryId,
                StartDate = movieVM.StartDate,
                EndDate = movieVM.EndDate,
                ImgUrl = movieVM.ImgUrl,
                TrailerUrl = movieVM.TrailerUrl
            };

            if (movieVM.StartDate > DateTime.Now)
            {
                newMovie.MovieStatus = MovieStatus.Upcoming;
            }
            else if (movieVM.EndDate < DateTime.Now)
            {
                newMovie.MovieStatus = MovieStatus.Expired;
            }
            else
            {
                newMovie.MovieStatus = MovieStatus.Available;
            }

            context.Movies.Add(newMovie);
            context.SaveChanges(); 

            foreach (var actorId in movieVM.MovieActors)
            {
                context.ActorMovies.Add(new ActorMovie { ActorId = actorId, MovieId = newMovie.Id });
            }

            context.SaveChanges(); 
        }


        public void Delete(int id)
        {
            var product = context.Movies.Find(id);
            if(product != null)
            {
                context.Movies.Remove(product);
                context.SaveChanges();
            }
        }

        public void Update(MoviesViewModel movieVM)
        {
            var dbMovie = context.Movies.Find(movieVM.Id);
            if(dbMovie != null) 
            { 
                dbMovie.Name = movieVM.Name;
                dbMovie.Price = movieVM.Price;
                dbMovie.StartDate = movieVM.StartDate;
                dbMovie.EndDate = movieVM.EndDate;
                dbMovie.ImgUrl = movieVM.ImgUrl;
                dbMovie.TrailerUrl = movieVM.TrailerUrl;
                dbMovie.CategoryId = movieVM.CategoryId;
                dbMovie.CinemaId = movieVM.CinemaId;
                if (movieVM.StartDate > DateTime.Now)
                {
                    dbMovie.MovieStatus = MovieStatus.Upcoming;
                }
                else if (movieVM.EndDate < DateTime.Now)
                {
                    dbMovie.MovieStatus = MovieStatus.Expired;
                }
                else
                {
                    dbMovie.MovieStatus = MovieStatus.Available;
                }

                var currentActors = context.ActorMovies.Where(e => e.MovieId == dbMovie.Id);
                context.RemoveRange(currentActors);
                
                foreach (var actorId in movieVM.MovieActors)
                {
                    context.ActorMovies.Add(new ActorMovie { ActorId = actorId, MovieId = dbMovie.Id });
                }
                context.SaveChanges();
            }
        }

        public List<Actor> GetMovieCast(int movieId)
        {
            return context.Actors
                .Where(actor => context.ActorMovies.Any(e => e.MovieId == movieId && e.ActorId == actor.Id))
                .ToList();
        }
        
        public void UpdateViewCount(int movieId)
        {
            var movie = ReadById(movieId);
            if(movie != null)
            {
                movie.ViewCount++;
                context.SaveChanges();
            }
        }

        public List<Movie> ReadWithCategAndCinema(int id)
        {
            return context.Movies.Include("Category").Include("Cinema").Where(e => e.CinemaId == id).ToList();
        }


        public List<int> GetAlreadySelectedActors(MoviesViewModel movieVM)
        {
            return movieVM.MovieActors = context.ActorMovies.Where(am => am.MovieId == movieVM.Id).Select(am => am.ActorId).ToList();
        }
    }
}
