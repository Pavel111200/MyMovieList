using Microsoft.EntityFrameworkCore;
using MyMovieList.Core.Contracts;
using MyMovieList.Core.Models;
using MyMovieList.Infrastructure.Data;
using MyMovieList.Infrastructure.Data.Repositories;

namespace MyMovieList.Core.Services
{
    public class MovieService : IMovieService
    {
        private readonly IApplicationDbRepository repo;

        public MovieService(IApplicationDbRepository repo)
        {
            this.repo = repo;
        }

        public async Task<bool> AddMovie(AddMovieViewModel model)
        {
            bool isSaved = false;
            List<Genre> genres = new List<Genre>();
            List<Writer> writers = new List<Writer>();
            List<Director> directors = new List<Director>();

            if (repo.All<Movie>().Any(m => model.Title == m.Title))
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
            foreach (var director in model.Director.Split(", "))
            {
                string firstName = director.Split(" ")[0];
                string lastName = director.Split(" ")[1];

                var directorToAdd = await repo.All<Director>()
                    .FirstOrDefaultAsync(d => d.Firstname == firstName && d.Lastname == lastName);

                if (directorToAdd != null)
                {
                    directors.Add(directorToAdd);
                }
                else
                {
                    directors.Add(new Director { Firstname = firstName, Lastname = lastName });
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
            var movie = new Movie
            {
                CreatedOn = model.CreatedOn,
                Title = model.Title,
                Description = model.Description,
                Genre = genres,
                Director = directors,
                Writer = writers,
                Runtime = model.Runtime,
                Image = model.Image
            };
            try
            {
                await repo.AddAsync<Movie>(movie);
                await repo.SaveChangesAsync();
                isSaved = true;
            }
            catch (Exception)
            {
            }
            return isSaved;
        }

        public async Task<IEnumerable<AllMoviesViewModel>> GetAllMovies()
        {
            var allMovies = await repo.All<Movie>()
                .Select(movie => new AllMoviesViewModel()
                {
                    Id = movie.Id,
                    Image = movie.Image,
                    Title = movie.Title,
                })
                .ToListAsync();

            foreach (var movie in allMovies)
            {
                movie.Rating = await GetRating(movie.Id.ToString());
                movie.Rating = Math.Round(movie.Rating, 1);
            }

            return allMovies;
        }

        public async Task<IEnumerable<GenreViewModel>> GetGenres()
        {
            return await repo.All<Genre>()
                .Select(x => new GenreViewModel { Genre = x.Name })
                .ToListAsync();
        }

        public async Task<MovieDetailsViewModel> GetMovieDetails(string id)
        {
            double rating = await GetRating(id);
            rating = Math.Round(rating, 1);

            return await repo.All<Movie>()
                .Where(m => id == m.Id.ToString())
                .Select(m => new MovieDetailsViewModel()
                {
                    Id = m.Id,
                    CreatedOn = m.CreatedOn,
                    Description = m.Description,
                    Director = string.Join(", ", m.Director.Select(d => $"{d.Firstname} {d.Lastname}")),
                    Genre = string.Join(", ", m.Genre.Select(g => g.Name)),
                    Image = m.Image,
                    Runtime = m.Runtime,
                    Title = m.Title,
                    Writer = string.Join(", ", m.Writer.Select(w => $"{w.Firstname} {w.Lastname}")),
                    Rating = rating
                })
                .FirstAsync();
        }

        public async Task<EditMovieViewModel> GetMovieForEdit(string id)
        {
            return await repo.All<Movie>()
                .Where(m => m.Id.ToString() == id)
                .Select(m => new EditMovieViewModel()
                {
                    Id = m.Id,
                    CreatedOn = m.CreatedOn,
                    Description = m.Description,
                    Director = string.Join(", ", m.Director.Select(d => $"{d.Firstname} {d.Lastname}")),
                    Genre = string.Join(", ", m.Genre.Select(g => g.Name)),
                    Image = m.Image,
                    Runtime = m.Runtime,
                    Title = m.Title,
                    Writer = string.Join(", ", m.Writer.Select(w => $"{w.Firstname} {w.Lastname}")),
                })
                .FirstAsync();
        }

        public async Task<IEnumerable<AllMoviesViewModel>> GetTopThree()
        {
            var allMovies = await repo.All<Movie>()
                .Select(movie => new AllMoviesViewModel()
                {
                    Id = movie.Id,
                    Image = movie.Image,
                    Title = movie.Title,
                })
                .ToListAsync();

            foreach (var movie in allMovies)
            {
                movie.Rating = await GetRating(movie.Id.ToString());
                movie.Rating = Math.Round(movie.Rating, 1);
            }

            return allMovies.OrderByDescending(m=>m.Rating).Take(3).ToList();
        }

        public async Task<bool> RateMovie(string userId, Guid movieId, double rating)
        {
            bool isSaved = false;
            var movieRating = await repo.All<MovieRating>()
                .FirstOrDefaultAsync(mr => mr.UserId == userId && mr.MovieId == movieId);

            if (movieRating != null)
            {
                movieRating.Rating = rating;
                await repo.SaveChangesAsync();
                return true;
            }

            MovieRating newMovieRating = new MovieRating()
            {
                UserId = userId,
                MovieId = movieId,
                Rating = rating,
            };
            try
            {
                await repo.AddAsync<MovieRating>(newMovieRating);
                await repo.SaveChangesAsync();
                isSaved = true;
            }
            catch (Exception)
            {
            }
            
            return isSaved;
        }

        public async Task<bool> UpdateMovie(EditMovieViewModel model)
        {
            bool isSaved = false;
            List<Genre> genres = new List<Genre>();
            List<Writer> writers = new List<Writer>();
            List<Director> directors = new List<Director>();

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
            foreach (var director in model.Director.Split(", "))
            {
                string firstName = director.Split(" ")[0];
                string lastName = director.Split(" ")[1];

                var directorToAdd = await repo.All<Director>()
                    .FirstOrDefaultAsync(d => d.Firstname == firstName && d.Lastname == lastName);

                if (directorToAdd != null)
                {
                    directors.Add(directorToAdd);
                }
                else
                {
                    directors.Add(new Director { Firstname = firstName, Lastname = lastName });
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

            var movieToUpdate = await repo.All<Movie>()
                .Include(m => m.Director)
                .Include(m => m.Genre)
                .Include(m => m.Writer)
                .FirstOrDefaultAsync(m => m.Id == model.Id);

            if (movieToUpdate != null)
            {
                movieToUpdate.Director = directors;
                movieToUpdate.Writer = writers;
                movieToUpdate.Genre = genres;
                movieToUpdate.Title = model.Title;
                movieToUpdate.Description = model.Description;
                movieToUpdate.CreatedOn = model.CreatedOn;
                movieToUpdate.Image = model.Image;
                movieToUpdate.Runtime = model.Runtime;
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

        private async Task<double> GetRating(string movieId)
        {
            List<double> allRatings = await repo.All<MovieRating>()
                 .Where(m => movieId == m.MovieId.ToString())
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
