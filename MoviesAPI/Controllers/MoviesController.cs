using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MoviesAPI.DTO;
using MoviesAPI.Models;
using System;

namespace MoviesAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        private new List<string> _allowedExtentions = new List<string> { ".jpg", ".png" };

        // 1024*1024*5===>5mb
        private long _maxAllowedSize = 5242880;
        public MoviesController(ApplicationDbContext db)
        {
            _db = db;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllMovies()
        {
            var movies=await _db.Movies
                .OrderByDescending(x => x.Rate)
                .Include(g=>g.Genre)
                .Select(g=>new MoviesDetailsDTO {
                    Id=g.Id,
                    GenreId=g.GenreId,
                    GenreName=g.Genre.Name,
                    StoreLine=g.StoreLine,
                    Poster= g.Poster,
                    Rate=g.Rate,
                    Title=g.Title,  
                    Year=g.Year,    
                })
                .ToListAsync();
            return Ok(movies);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetMovieById(int id)
        {
            var movie=await _db.Movies.Include(g=>g.Genre).SingleOrDefaultAsync(m=>m.Id==id);
            if(movie==null)
                return NotFound();
            var dto = new MoviesDetailsDTO
            {
                Id = movie.Id,
                Title = movie.Title,
                Year = movie.Year,
                Rate = movie.Rate,
                Poster = movie.Poster,
                StoreLine = movie.StoreLine,
                GenreId = movie.GenreId,
                GenreName = movie.Genre.Name,

            };
            return Ok(movie.Genre.Name);
            //return Ok(movie);
        }
        [HttpPost]

        public async Task<IActionResult> CreateMovie([FromForm]MovieDTO dto)
        {
            if (dto.Poster == null)
                return BadRequest("poster is required");


            if (!_allowedExtentions.Contains(Path.GetExtension(dto.Poster.FileName).ToLower()))
                return BadRequest("Only allowed Extentions is jpg or png");
            if(dto.Poster.Length>_maxAllowedSize)
                return BadRequest("size of poster must be less than 5mb");
            var isValidGenre= _db.Genres.Any(g=>g.Id==dto.GenreId);
            if (!isValidGenre)
                return BadRequest("invalid genre Id");
          

            using var dataStream = new MemoryStream();
            await dto.Poster.CopyToAsync(dataStream);
            var movie = new Movie
            {
                GenreId = dto.GenreId,
                Title = dto.Title,
                Rate = dto.Rate,
                Year = dto.Year,
                Poster = dataStream.ToArray(),
                StoreLine = dto.StoreLine,
            };
            await _db.Movies.AddAsync(movie);
            _db.SaveChanges();
            return Ok(movie);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMovieAsync(int id)
        {
            var movie = await _db.Movies.FindAsync(id);
            if (movie == null)
                return BadRequest("movie is not found");
            _db.Movies.Remove(movie);
            _db.SaveChanges();
            return Ok(movie);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateGenre(int id, [FromForm]MovieDTO dto)
        {
            var movie = await _db.Movies.Include(c => c.Genre).FirstOrDefaultAsync(m => m.Id == id);
            if (movie == null)
                return NotFound();
            var isValidGenre = _db.Genres.Any(g => g.Id == dto.GenreId);

            if (!isValidGenre)
                return BadRequest("invalid genre Id");
          

            if (dto.Poster != null)
            {
                if (_maxAllowedSize < dto.Poster.Length)
                    return BadRequest("poster Image is very large");
                if (!_allowedExtentions.Contains(Path.GetExtension(dto.Poster.FileName)))
                    return BadRequest("Error in Extention");


                var dataStream = new MemoryStream();
                await dto.Poster.CopyToAsync(dataStream);
                movie.Poster = dataStream.ToArray();
            }
          

           
            movie.Title = dto.Title;
      
            movie.StoreLine=dto.StoreLine;  
            movie.Rate = dto.Rate;
            movie.Year = dto.Year;
            movie.GenreId = dto.GenreId;
           // movie.Genre.Name = dto.GenreName;

            _db.SaveChanges();
            return Ok(movie);

        }
    }
}
