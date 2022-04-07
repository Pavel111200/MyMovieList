using Microsoft.EntityFrameworkCore;
using MyMovieList.Core.Contracts;
using MyMovieList.Core.Models;
using MyMovieList.Infrastructure.Data;
using MyMovieList.Infrastructure.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

            if(repo.All<Movie>().Any(m=> model.Title == m.Title))
            {
                return true;
            }

            foreach (var genre in model.Genre.Split(", "))
            {
                if(await repo.All<Genre>().FirstOrDefaultAsync(g=> genre== g.Name) != null)
                {
                    genres.Add(await repo.All<Genre>().FirstOrDefaultAsync(g => genre == g.Name));
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
                if (repo.All<Director>().Where(d=> d.Firstname==firstName && d.Lastname==lastName) !=null)
                {
                    directors.Add(await repo.All<Director>().Where(d => d.Firstname == firstName && d.Lastname == lastName).FirstAsync());
                }
                else
                {
                    directors.Add(new Director { Firstname=firstName, Lastname=lastName});
                }
            }
            foreach (var writer in model.Writer.Split(", "))
            {
                string firstName = writer.Split(" ")[0];
                string lastName = writer.Split(" ")[1];
                if (repo.All<Writer>().Where(d => d.Firstname == firstName && d.Lastname == lastName) != null)
                {
                    writers.Add(await repo.All<Writer>().Where(d => d.Firstname == firstName && d.Lastname == lastName).FirstAsync());
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
                Runtime=model.Runtime,
                Image=model.Image
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

            return await repo.All<Movie>()
                .Where(m => id == m.Id.ToString())
                .Select(m => new MovieDetailsViewModel()
                {
                    Id = m.Id,
                    CreatedOn = m.CreatedOn,
                    Description = m.Description,
                    Director = string.Join(", ",m.Director.Select(d=> $"{d.Firstname} {d.Lastname}")),
                    Genre = string.Join(", ",m.Genre.Select(g=>g.Name)),
                    Image = m.Image,
                    Runtime = m.Runtime,
                    Title = m.Title,
                    Writer = string.Join(", ", m.Writer.Select(w => $"{w.Firstname} {w.Lastname}")),
                    Rating = rating
                })
                .FirstAsync();
        }

        private async Task<double> GetRating(string movieId)
        {
           List<double> allRatings = await repo.All<MovieRating>()
                .Where(m=>movieId==m.MovieId.ToString())
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

            return sumOfRatings/allRatings.Count;
        } 
    }
}
