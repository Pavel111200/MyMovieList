using Microsoft.EntityFrameworkCore;
using MyMovieList.Core.Contracts;
using MyMovieList.Core.Models;
using MyMovieList.Infrastructure.Data;
using MyMovieList.Infrastructure.Data.Repositories;

namespace MyMovieList.Core.Services
{
    public class TVShowService : ITVShowService
    {
        private readonly IApplicationDbRepository repo;

        public TVShowService(IApplicationDbRepository repo)
        {
            this.repo = repo;
        }

        public async Task<bool> AddShow(AddTVShowViewModel model)
        {
            bool isSaved = false;
            List<Genre> genres = new List<Genre>();
            List<Writer> writers = new List<Writer>();

            if (repo.All<TVShow>().Any(s => model.Title == s.Title))
            {
                return true;
            }

            foreach (var genre in model.Genre.Split(", "))
            {
                var genreToAdd = await repo.All<Genre>().FirstOrDefaultAsync(g => genre == g.Name);
                if (genreToAdd != null)
                {
                    genres.Add(genreToAdd);
                }
                else
                {
                    genres.Add(new Genre { Name = genre });
                }
            }

            foreach (var writer in model.Writer.Split(", "))
            {
                string firstName = writer.Split(" ")[0];
                string lastName = writer.Split(" ")[1];

                var writerToAdd = await repo.All<Writer>()
                    .FirstOrDefaultAsync(w => w.Firstname == firstName && w.Lastname == lastName);

                if (writerToAdd != null)
                {
                    writers.Add(writerToAdd);
                }
                else
                {
                    writers.Add(new Writer { Firstname = firstName, Lastname = lastName });
                }
            }
            var show = new TVShow
            {
                Description = model.Description,
                Genre = genres,
                Image = model.Image,
                NumberOfEpisodes = model.NumberOfEpisodes,
                Title = model.Title,
                Season = model.Season,
                Writer = writers
            };
            try
            {
                await repo.AddAsync<TVShow>(show);
                await repo.SaveChangesAsync();
                isSaved = true;
            }
            catch (Exception)
            {
            }
            return isSaved;
        }

        public async Task<IEnumerable<AllTVShowsViewModel>> GetAllTVShows()
        {
            var shows = await repo.All<TVShow>()
            .Select(s => new AllTVShowsViewModel
            {
                Id = s.Id,
                Season = s.Season.ToString(),
                Image = s.Image,
                Title = s.Title
            })
            .ToListAsync();

            foreach (var show in shows)
            {
                show.Rating = await GetRating(show.Id.ToString());
            }

            return shows;
        }

        public async Task<IEnumerable<GenreViewModel>> GetGenres()
        {
            return await repo.All<Genre>()
                .Select(x => new GenreViewModel { Genre = x.Name })
                .ToListAsync();
        }

        private async Task<double> GetRating(string showId)
        {
            List<double> allRatings = await repo.All<TVShowRating>()
                 .Where(s => showId == s.TVShowId.ToString())
                 .Select(x => x.Rating)
                 .ToListAsync();

            if (allRatings.Count == 0)
            {
                return 1.00;
            }

            double sumOfRatings = 0.00;
            foreach (var rating in allRatings)
            {
                sumOfRatings += rating;
            }

            return sumOfRatings / allRatings.Count;
        }
    }
}
