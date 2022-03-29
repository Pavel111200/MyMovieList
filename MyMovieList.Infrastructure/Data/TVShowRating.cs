using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyMovieList.Infrastructure.Data
{
    public class TVShowRating
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        [Range(1, 10)]
        public double Rating { get; set; }

        [Required]
        [ForeignKey(nameof(User))]
        public string UserId { get; set; }

        public IdentityUser User { get; set; }

        [Required]
        [ForeignKey(nameof(TVShow))]
        public Guid TVShowId { get; set; }

        public TVShow TVShow { get; set; }
    }
}
