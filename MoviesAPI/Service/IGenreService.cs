using MoviesAPI.Models;

namespace MoviesAPI.Service
{
    public interface IRepoService<TEntity>
    {
        Task<IEnumerable<TEntity>> GetAll();
        Task<TEntity> GetById(int id);
        Task<IEnumerable<Movie>> GetMovies_Genre(int GenreId);
        Task<TEntity> Add(TEntity genre);
        TEntity Update(TEntity genre);
        TEntity Delete(TEntity genre);
       
    }
}
