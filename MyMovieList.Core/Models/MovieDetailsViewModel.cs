using MyMovieList.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyMovieList.Core.Models
{
    public class MovieDetailsViewModel
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        [StringLength(210)]
        public string Title { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        [StringLength(200)]
        public string Image { get; set; }

        public TimeSpan Runtime { get; set; }

        public DateTime CreatedOn { get; set; }

        public double Rating { get; set; }

        public string Writer { get; set; }

        public string Director { get; set; }

        public string Genre { get; set; }
    }
}
