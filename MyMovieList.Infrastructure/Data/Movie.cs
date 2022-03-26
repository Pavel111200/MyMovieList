using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyMovieList.Infrastructure.Data
{
    public class Movie
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

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
        [Column(TypeName ="date")]
        public DateTime CreatedOn { get; set; }

        public ICollection<Writer> Writer { get; set; } = new List<Writer>();
        public ICollection<Director> Director { get; set; } = new List<Director>();
        public ICollection<Genre> Genre { get; set; } = new List<Genre>();
    }
}
