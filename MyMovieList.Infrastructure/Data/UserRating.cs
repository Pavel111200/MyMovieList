using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyMovieList.Infrastructure.Data
{
    public class UserRating
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        public double Rating { get; set; }

        [Required]
        [ForeignKey(nameof(User))]
        public string UserId { get; set; }

        public IdentityUser User { get; set; }

        [Required]
        [ForeignKey(nameof(Movie))]
        public Guid MovieId { get; set; }

        public Movie Movie { get; set; }

        [Required]
        [ForeignKey(nameof(TvShow))]
        public Guid TVShowId { get; set; }

        public TVShow TvShow { get; set; }
    }
}
