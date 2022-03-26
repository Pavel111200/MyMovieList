using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyMovieList.Infrastructure.Data
{
    public class DirectorMovie
    {
        [ForeignKey(nameof(Movie))]
        public Guid MovieId { get; set; }

        public Movie Movie { get; set; }

        [ForeignKey(nameof(Director))]
        public Guid DirectorId { get; set; }

        public Director Director { get; set; }
    }
}
