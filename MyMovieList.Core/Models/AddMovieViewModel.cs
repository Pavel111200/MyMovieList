using MyMovieList.Infrastructure.Data;
using System.ComponentModel.DataAnnotations;

namespace MyMovieList.Core.Models
{
    public class AddMovieViewModel
    {
        [Required]
        [StringLength(210)]
        public string Title { get; set; }

        [Required]
        [StringLength(500)]
        public string Description { get; set; }

        [Required]
        [StringLength(200)]
        public string Image { get; set; }

        [Required]
        public TimeSpan Runtime { get; set; }

        [Required]
        public DateTime CreatedOn { get; set; }

        [Required]
        public string Writer { get; set; }

        [Required]
        public string Director { get; set; }

        [Required]
        public string Genre { get; set; }
    }
}
