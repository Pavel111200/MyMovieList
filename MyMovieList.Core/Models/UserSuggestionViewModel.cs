using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyMovieList.Core.Models
{
    public class UserSuggestionViewModel
    {
        [Required]
        [StringLength(210)]
        public string Title { get; set; }

        [Required]
        [StringLength(6)]
        public string Type { get; set; }
    }
}
