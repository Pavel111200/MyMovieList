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

        Task<bool> UpdateTVShow(EditTVShowViewModel model);

        Task<EditTVShowViewModel> GetTVShowForEdit(string id);

        Task<bool> RateShow(string userId, Guid showId, double rating);
    }
}
