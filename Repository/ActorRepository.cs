using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Movies_Point.Data;
using Movies_Point.IRepository;
using Movies_Point.Models;
using Movies_Point.ViewModels;

namespace Movies_Point.Repository
{
    public class ActorRepository : IActorRepository
    { 
        ApplicationDbContext context;
        public ActorRepository(ApplicationDbContext context)
        {
            this.context = context;
        }
        public List<Actor> ReadAll()
        {
            return context.Actors.ToList();
        }
        public Actor ReadById(int id)
        {
            return context.Actors.Find(id);
        }
        
        public List<Movie> GetActorMovies(int actorId)
        {
            var actorMovies = context.ActorMovies
             .Where(e => e.ActorId == actorId)
             .Select(e => e.MovieId);

            var movies = context.Movies.Include("Category").Include("Cinema")
                .Where(e => actorMovies.Contains(e.Id))
                .ToList();
            return movies;
        }

        public void Create(ActorViewModel actorVM)
        {
            Actor actor = new Actor();
            actor.FirstName = actorVM.FirstName;
            actor.LastName = actorVM.LastName;
            actor.Bio = actorVM.Bio;
            actor.ProfilePicture = actorVM.ProfilePicture;
            actor.News = actorVM.News;
            
            context.Actors.Add(actor);
            context.SaveChanges();
            
            foreach(var movieId in actorVM.MoviesIds)
            {
                context.ActorMovies.Add(new ActorMovie { ActorId = actor.Id, MovieId = movieId });
                context.SaveChanges();
            }
        }
        public void Update(ActorViewModel actorVM)
        {
            Actor? actor = context.Actors.Find(actorVM.Id);
            if(actor != null)
            {
                actor.FirstName = actorVM.FirstName;
                actor.LastName = actorVM.LastName;
                actor.Bio = actorVM.Bio;
                actor.ProfilePicture = actorVM.ProfilePicture;
                actor.News = actorVM.News;
                context.SaveChanges();

                var existingMoviesandActors = context.ActorMovies.Where(am => am.ActorId == actor.Id);
                context.ActorMovies.RemoveRange(existingMoviesandActors);

                foreach (var movieId in actorVM.MoviesIds)
                {
                    context.ActorMovies.Add(new ActorMovie { ActorId = actor.Id, MovieId = movieId });
                }

                context.SaveChanges();
            }

        }
        public bool Delete(int id)
        {
            var actor = context.Actors.Find(id);
            if (actor != null)
            {
                context.Actors.Remove(actor);
                context.SaveChanges();
                return true;
            }
            return false;
        }


        public List<int> GetAlreadySelectedMovies(ActorViewModel actorVM)
        {
            return actorVM.MoviesIds = context.ActorMovies.Where(e=>e.ActorId == actorVM.Id).Select(e=>e.MovieId).ToList();
        }
    }
}
