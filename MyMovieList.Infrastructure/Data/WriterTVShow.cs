using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyMovieList.Infrastructure.Data
{
    public class WriterTVShow
    {
        [ForeignKey(nameof(TVShow))]
        public Guid TVShowId { get; set; }

        public TVShow TVShow { get; set; }

        [ForeignKey(nameof(Writer))]
        public Guid WriterId { get; set; }

        public Writer Writer { get; set; }
    }
}
