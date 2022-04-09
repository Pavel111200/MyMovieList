using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyMovieList.Core.Models
{
    public class AddTVShowViewModel
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
        public int Season { get; set; }

        [Required]
        public int NumberOfEpisodes { get; set; }

        [Required]
        public string Writer { get; set; }

        [Required]
        public string Genre { get; set; }
    }
}
