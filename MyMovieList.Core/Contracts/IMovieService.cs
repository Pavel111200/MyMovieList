using MyMovieList.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyMovieList.Core.Contracts
{
    public interface IMovieService
    {
        Task<IEnumerable<GenreViewModel>> GetGenres();

        Task<bool> AddMovie(AddMovieViewModel model);
    }
}
