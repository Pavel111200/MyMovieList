using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyMovieList.Infrastructure.Data
{
    public class TVShow
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
        public int Season { get; set; }

        [Required]
        public int NumberOfEpisodes { get; set; }

        public ICollection<Writer> Writer { get; set; } = new List<Writer>();
        public ICollection<Genre> Genre { get; set; } = new List<Genre>();
    }
}
