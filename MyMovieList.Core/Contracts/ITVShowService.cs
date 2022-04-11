using MyMovieList.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyMovieList.Core.Contracts
{
    public interface ITVShowService
    {
        Task<IEnumerable<GenreViewModel>> GetGenres();

        Task<bool> AddShow(AddTVShowViewModel model);

        Task<IEnumerable<AllTVShowsViewModel>> GetAllTVShows();

        Task<TVShowDetailsViewModel> GetTVShowDetails(string id);

        //Task<bool> UpdateMovie(EditMovieViewModel model);

        //Task<EditMovieViewModel> GetMovieForEdit(string id);
    }
}
