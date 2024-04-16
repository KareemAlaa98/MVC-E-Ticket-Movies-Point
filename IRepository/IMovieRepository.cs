using Movies_Point.Models;
using Movies_Point.ViewModels;

namespace Movies_Point.IRepository
{
    public interface IMovieRepository
    {
        List<Movie> ReadAll();
        Movie ReadById(int id);
        Movie ReadById2(int id);
        void Create(MoviesViewModel movie);
        void Update(MoviesViewModel movie);
        void Delete(int id);
        List<Actor> GetMovieCast(int id);
        void UpdateViewCount(int id);
        List<Movie> ReadWithCategAndCinema(int it);

        List<int> GetAlreadySelectedActors(MoviesViewModel movieVM);
    }
}
