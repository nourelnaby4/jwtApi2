using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MoviesAPI.DTO;
using MoviesAPI.Models;
using MoviesAPI.Service;

namespace MoviesAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenresController : ControllerBase
    {
        private readonly IRepoService<Genre> _genreService;
        public GenresController(IRepoService<Genre> genreService)
        {
            _genreService = genreService;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllGenres()
        {
            var genres = await _genreService.GetAll();
            return Ok(genres);
        }
        [HttpPost]
        public async Task<IActionResult> CreateGenre([FromBody] GenreDTO dto)
        {
            //dto is data transfer object 
            //respose for link between data and api 
            //there are data user do not need to post it like id 
            // so we use dto


            var genre=new Genre { Name=dto.Name};
            await _genreService.Add(genre);
          
            return Ok(genre);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateGenre(int id,[FromBody] GenreDTO dto)
        {
            var genre = await _genreService.GetById(id);
            if (genre == null)
                return NotFound("Genre is not found");
            genre.Name = dto.Name;
           _genreService.Update(genre); 
            return Ok(genre);
           
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGenre(int id)
        {
            var genre = await _genreService.GetById(id);
            if (genre == null)
                return NotFound("Genre is not found");
            _genreService.Delete(genre);
            return Ok(genre);

        }
        [HttpGet("{GenreId}")]
        public async Task<IActionResult> GetMovies_Genre(int GenreId)
        {
            var movies_Genre = await _genreService.GetMovies_Genre(GenreId);
            return Ok(movies_Genre);
        }


    }
}
