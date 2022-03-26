using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyMovieList.Infrastructure.Data
{
    public class GenreMovie
    {
        [ForeignKey(nameof(Movie))]
        public Guid MovieId { get; set; }

        public Movie Movie { get; set; }

        [ForeignKey(nameof(Genre))]
        public Guid GenreId { get; set; }

        public Genre Genre { get; set; }
    }
}
