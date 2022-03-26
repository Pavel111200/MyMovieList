using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyMovieList.Infrastructure.Data
{
    public class Genre
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        public ICollection<Movie> Movies { get; set; } = new List<Movie>();
        public ICollection<TVShow> TVShows { get; set; } = new List<TVShow>();
    }
}
