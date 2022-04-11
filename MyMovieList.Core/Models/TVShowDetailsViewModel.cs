using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyMovieList.Core.Models
{
    public class TVShowDetailsViewModel
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public string Title { get; set; }

        public string Description { get; set; }

        public string Image { get; set; }

        public int Season { get; set; }

        public int NumberOfEpisodes { get; set; }

        public double Rating { get; set; }

        public string Writer { get; set; }
        public string Genre { get; set; }
    }
}
