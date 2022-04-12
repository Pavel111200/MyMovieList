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

        Task<IEnumerable<AllMoviesViewModel>> GetAllMovies();

        Task<MovieDetailsViewModel> GetMovieDetails(string id);

        Task<bool> UpdateMovie(EditMovieViewModel model);

        Task<EditMovieViewModel> GetMovieForEdit(string id);

        Task<IEnumerable<AllMoviesViewModel>> GetTopThree();

        Task<bool> RateMovie(string userId, Guid movieId, double rating);

        Task<IEnumerable<LikedMoviesViewModel>> GetLikedMovies(string userId);
    }
}
