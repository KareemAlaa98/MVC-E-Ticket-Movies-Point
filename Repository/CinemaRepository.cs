using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Movies_Point.Data;
using Movies_Point.IRepository;
using Movies_Point.Models;
using Movies_Point.ViewModels;

namespace Movies_Point.Repository
{
    public class CinemaRepository : ICinemaRepository
    {

        ApplicationDbContext context;
        public CinemaRepository(ApplicationDbContext context)
        {
            this.context = context;
        }

        public Cinema ReadById(int id)
        {
            return context.Cinemas.Find(id);
        }
        public List<Cinema> ReadAll()
        {
            return context.Cinemas.ToList();
        }

        public void Create(CinemaViewModel cinemaVM)
        {
            Cinema cinema = new Cinema() {
                Name = cinemaVM.Name, 
                Description = cinemaVM.Description,
                //CinemaLogo = cinemaVM.CinemaLogo,
                Address= cinemaVM.Address
            };
            context.Cinemas.Add(cinema);
            context.SaveChanges();

            foreach(var movieId in cinemaVM.MoviesIds)
            {
                context.Movies.FirstOrDefault(e => e.Id == movieId).CinemaId = cinema.Id;
                context.SaveChanges();
            }
        }

        public void Update(CinemaViewModel cinemaVM)
        {
            var cinema = context.Cinemas.Find(cinemaVM.Id);
            if(cinema != null)
            {
                cinema.Name = cinemaVM.Name;
                cinema.Description = cinemaVM.Description;
                cinema.Address = cinemaVM.Address;
                //cinema.CinemaLogo = cinemaVM.CinemaLogo;

                context.SaveChanges();

                foreach (var movieId in cinemaVM.MoviesIds)
                {
                    context.Movies.FirstOrDefault(e => e.Id == movieId).CinemaId = cinema.Id;
                    context.SaveChanges();
                }
            }
        }

        public bool Delete(int id)
        {
            var cinema = context.Cinemas.Find(id);
            if (cinema != null)
            {
                context.Cinemas.Remove(cinema);
                context.SaveChanges();
                return true;
            }
            return false;
        }

        public List<int> GetAlreadySelectedMovies(CinemaViewModel cinemaVM)
        {
           return cinemaVM.MoviesIds = context.Movies.Where(m => m.CinemaId == cinemaVM.Id).Select(m => m.Id).ToList();
        }
    }
}
