using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MoviesAPI.Models;

namespace MoviesAPI.Service
{
    public class GenreService : IRepoService<Genre>
    {
        private readonly ApplicationDbContext _db;
        public GenreService(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task<Genre> Add(Genre genre)
        {
           await _db.Genres.AddAsync(genre);
            _db.SaveChanges();
            return (genre);
            
        }
       

        public  Genre Delete(Genre genre)
        {
           _db.Genres.Remove(genre);
            _db.SaveChanges();
            return genre;
        }

        public async Task<Genre> GetById(int id)
        {
            return await _db.Genres.FirstOrDefaultAsync(g => g.Id == id);
        }

        public async Task<IEnumerable<Genre>> GetAll()
        {
            return await _db.Genres.OrderBy(g => g.Name).ToListAsync();
        }

        public  Genre Update(Genre genre)
        {
            _db.Genres.Update(genre);
            _db.SaveChanges();
            return genre;
        }

        public async Task<IEnumerable<Movie>> GetMovies_Genre(int GenreId)
        {

            var movies_Genre=await _db.Movies.Where(m=>m.GenreId==GenreId).ToListAsync();
            return movies_Genre;
          
                //var Movie_Genre = await _db.Movies.Where(m => m.GenreId == GenreId).ToListAsync();
                //return Movie_Genre;
           
        }
    }
}
