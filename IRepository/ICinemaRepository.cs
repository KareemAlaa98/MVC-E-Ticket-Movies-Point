using Movies_Point.Data;
using Movies_Point.Models;
using Movies_Point.ViewModels;

namespace Movies_Point.IRepository
{
    public interface ICinemaRepository
    {
        List<Cinema> ReadAll();
        Cinema ReadById(int id);
        void Create(CinemaViewModel cinemaVM);
        void Update(CinemaViewModel cinemaVM);
        bool Delete(int id);
        List<int> GetAlreadySelectedMovies(CinemaViewModel cinemaVM);

    }
}
