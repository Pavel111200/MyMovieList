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

            if (repo.All<TVShow>().Any(s => model.Title == s.Title && model.Season==s.Season))
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

        public async Task<IEnumerable<LikedShowsViewModel>> GetLikedShows(string userId)
        {
            var likedShows = await repo.All<TVShowRating>()
                .Where(sr => sr.UserId == userId)
                .Select(sr => new LikedShowsViewModel
                {
                    Id = sr.TVShow.Id,
                    Image = sr.TVShow.Image,
                    Title = sr.TVShow.Title,
                    YourRating = sr.Rating,
                    Season = sr.TVShow.Season,
                })
                .ToListAsync();

            foreach (var show in likedShows)
            {
                show.OverallRating = await GetRating(show.Id.ToString());
                show.OverallRating = Math.Round(show.OverallRating, 1);
            }

            return likedShows;
        }

        public async Task<TVShowDetailsViewModel> GetTVShowDetails(string id)
        {
            var show = await repo.All<TVShow>()
                .Include(s => s.Genre)
                .Include(s => s.Writer)
                .FirstOrDefaultAsync(m => m.Id.ToString() == id);

            if (show == null)
            {
                throw new ArgumentException("The given id doesn't exist.");
            }

            double rating = await GetRating(id);
            rating = Math.Round(rating, 1);

            return new TVShowDetailsViewModel()
            {
                Id = show.Id,
                Description = show.Description,
                Genre = string.Join(", ", show.Genre.Select(g => g.Name)),
                Image = show.Image,
                Title = show.Title,
                Writer = string.Join(", ", show.Writer.Select(w => $"{w.Firstname} {w.Lastname}")),
                Rating = rating,
                NumberOfEpisodes = show.NumberOfEpisodes,
                Season = show.Season
            };
        }

        public async Task<EditTVShowViewModel> GetTVShowForEdit(string id)
        {
            var show = await repo.All<TVShow>()
                .Include(s=> s.Genre)
                .Include(s=>s.Writer)
                .FirstOrDefaultAsync(m => m.Id.ToString() == id);

            if (show == null)
            {
                throw new ArgumentException("The given id doesn't exist.");
            }

            return new EditTVShowViewModel()
            {
                Id = show.Id,
                Description = show.Description,
                Genre = string.Join(", ", show.Genre.Select(g => g.Name)),
                Image = show.Image,
                Title = show.Title,
                Writer = string.Join(", ", show.Writer.Select(w => $"{w.Firstname} {w.Lastname}")),
                Season = show.Season,
                NumberOfEpisodes = show.NumberOfEpisodes,
            };
        }

        public async Task<bool> RateShow(string userId, Guid showId, double rating)
        {
            bool isSaved = false;
            var showRating = await repo.All<TVShowRating>()
                .FirstOrDefaultAsync(sr => sr.UserId == userId && sr.TVShowId == showId);

            if (showRating != null)
            {
                showRating.Rating = rating;
                await repo.SaveChangesAsync();
                return true;
            }

            TVShowRating newShowRating = new TVShowRating()
            {
                UserId = userId,
                TVShowId = showId,
                Rating = rating,
            };
            try
            {
                await repo.AddAsync<TVShowRating>(newShowRating);
                await repo.SaveChangesAsync();
                isSaved = true;
            }
            catch (Exception)
            {
            }

            return isSaved;
        }

        public async Task<bool> UpdateTVShow(EditTVShowViewModel model)
        {
            bool isSaved = false;
            List<Genre> genres = new List<Genre>();
            List<Writer> writers = new List<Writer>();

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

            var showToUpdate = await repo.All<TVShow>()
                .Include(s => s.Genre)
                .Include(s => s.Writer)
                .FirstOrDefaultAsync(s => s.Id == model.Id);

            if (showToUpdate != null)
            {
                showToUpdate.Writer = writers;
                showToUpdate.Genre = genres;
                showToUpdate.Title = model.Title;
                showToUpdate.Description = model.Description;
                showToUpdate.Season = model.Season;
                showToUpdate.Image = model.Image;
                showToUpdate.NumberOfEpisodes = model.NumberOfEpisodes;
            }
            try
            {
                await repo.SaveChangesAsync();
                isSaved = true;
            }
            catch (Exception)
            {
            }
            return isSaved;
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
