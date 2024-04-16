using Movies_Point.Models;
using Movies_Point.ViewModels;

namespace Movies_Point.IRepository
{
    public interface IActorRepository
    {
        List<Actor> ReadAll();
        Actor ReadById(int id);
        List<Movie> GetActorMovies(int id);

        void Create(ActorViewModel actorVM);
        void Update(ActorViewModel actorVM);
        bool Delete(int id);

        List<int> GetAlreadySelectedMovies(ActorViewModel actorVM);
    }
}
