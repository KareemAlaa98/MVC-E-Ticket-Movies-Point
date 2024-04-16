using Movies_Point.Models;
using Movies_Point.ViewModels;

namespace Movies_Point.IRepository
{
    public interface ICategoryRepository
    {
        List<Category> ReadAll();
        Category ReadById(int id);

        void Create(CategoryViewModel categoryVM);
        void Update(CategoryViewModel categoryVM);
        void Delete(int id);
        List<Movie> getCategoryMovies(int id);
    }
}
