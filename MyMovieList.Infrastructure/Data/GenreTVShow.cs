using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyMovieList.Infrastructure.Data
{
    public class GenreTVShow
    {
        [ForeignKey(nameof(TVShow))]
        public Guid TVShowId { get; set; }

        public TVShow TVShow { get; set; }

        [ForeignKey(nameof(Genre))]
        public Guid GenreId { get; set; }

        public Genre Genre { get; set; }
    }
}
