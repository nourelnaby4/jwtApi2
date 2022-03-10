

namespace MoviesAPI.Models
{
    public class Genre
    {

        public int Id { get; set; }
        [MaxLength(100)]
        public string Name { get; set; }
        public List<Movie> Movies { get; set; }
    }
}
