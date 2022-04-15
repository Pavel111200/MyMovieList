using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using MyMovieList.Core.Contracts;
using MyMovieList.Core.Models;
using MyMovieList.Core.Services;
using MyMovieList.Infrastructure.Data;
using MyMovieList.Infrastructure.Data.Repositories;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyMovieList.Test
{
    public class MovieServiceTest
    {
        private ServiceProvider serviceProvider;
        private InMemoryDbContext dbContext;

        [SetUp]
        public async Task Setup()
        {
            dbContext = new InMemoryDbContext();
            var serviceCollection = new ServiceCollection();

            serviceProvider = serviceCollection
                .AddSingleton(sp => dbContext.CreateContext())
                .AddSingleton<IApplicationDbRepository, ApplicationDbRepository>()
                .AddSingleton<IMovieService, MovieService>()
                .BuildServiceProvider();

            var repo = serviceProvider.GetService<IApplicationDbRepository>();
            await SeedMovie(repo);
        }

        [Test]
        public async Task AddMovieReturnsTrueIfValid()
        {
            var movieService = serviceProvider.GetService<IMovieService>();

            var validMovie = new AddMovieViewModel()
            {
                Title = "Harry Poter",
                Description = "soafnsfasjfaosjfiajsiooaogahdgaod",
                Genre = "Fantasy",
                Director = "Pesho Peshev",
                Writer = "Gosho Goshev",
                CreatedOn = DateTime.Now,
                Runtime = TimeSpan.FromHours(2),
                Image = "Harrypoter.png"
            };
            bool isSaved = await movieService.AddMovie(validMovie);
            Assert.IsTrue(isSaved);
        }

        [Test]
        public async Task GetAllMoviesReturnsCorrectMovies()
        {
            var movieService = serviceProvider.GetService<IMovieService>();

            var expected = new AllMoviesViewModel()
            {

                Id = new Guid("ac9470b4-1e07-4699-858c-8cb74f86fd9a"),
                Title = "Harry Poter",
                Rating = 1.0,
                Image = "Harrypoter.png"

            };
            var result = await movieService.GetAllMovies();
            var actual = result.First();

            Assert.IsTrue(expected.Title == actual.Title &&
                expected.Image == actual.Image &&
                expected.Id == actual.Id &&
                expected.Rating == actual.Rating);
        }

        [Test]
        public async Task GetGenresReturnsCorrectGenres()
        {
            var movieService = serviceProvider.GetService<IMovieService>();

            var expected = new GenreViewModel
            {
                Genre = "Fantasy"
            };
            var result = await movieService.GetGenres();
            var actual = result.First();

            Assert.AreEqual(expected.Genre,actual.Genre);
        }

        [Test]
        public async Task GetLikedMoviesReturnsTheCorrectMovies()
        {
            var movieService = serviceProvider.GetService<IMovieService>();
            string userId = "f87d224a-02cd-41cc-af7f-a1f430e2ce8e";

            var expected = new LikedMoviesViewModel()
            {
                Id = new Guid("ac9470b4-1e07-4699-858c-8cb74f86fd9a"),
                Image = "Harrypoter.png",
                Title = "Harry Poter",
                OverallRating = 1,
                YourRating = 10
            };

            var result = await movieService.GetLikedMovies(userId);
            var actual = result.First();

            Assert.IsTrue(expected.Title == actual.Title &&
                expected.Image == actual.Image &&
                expected.Id == actual.Id &&
                expected.OverallRating == actual.OverallRating &&
                expected.YourRating == actual.YourRating);
        }

        //[Test]
        //public async Task GetMovieDetailsReturnsCorrectObject()
        //{
        //    var movieService = serviceProvider.GetService<IMovieService>();
        //    string movieId = "ac9470b4-1e07-4699-858c-8cb74f86fd9a";

        //    var expected = new MovieDetailsViewModel()
        //    {
        //        Id = new Guid("ac9470b4-1e07-4699-858c-8cb74f86fd9a"),
        //        Title = "Harry Poter",
        //        Description = "soafnsfasjfaosjfiajsiooaogahdgaod",
        //        Genre = "Fantasy",
        //        Director = "Pesho Peshev",
        //        Writer = "Gosho Goshev",
        //        CreatedOn = DateTime.Now,
        //        Runtime = TimeSpan.FromHours(2),
        //        Image = "Harrypoter.png",
        //        Rating = 1.0,
        //    };

        //    var actual = await movieService.GetMovieDetails(movieId);

        //    Assert.IsTrue(expected.Title == actual.Title &&
        //        expected.Image == actual.Image &&
        //        expected.Id == actual.Id &&
        //        expected.Director == actual.Director &&
        //        expected.Writer == actual.Writer &&
        //        expected.CreatedOn == actual.CreatedOn &&
        //        expected.Runtime == actual.Runtime &&
        //        expected.Genre == actual.Genre &&
        //        expected.Description == actual.Description &&
        //        expected.Rating == actual.Rating);
        //}

        //[Test]
        //public async Task GetMovieForEditReturnsCorrectObject()
        //{
        //    var movieService = serviceProvider.GetService<IMovieService>();
        //    string movieId = "ac9470b4-1e07-4699-858c-8cb74f86fd9a";

        //    var expected = new EditMovieViewModel()
        //    {
        //        Id = new Guid("ac9470b4-1e07-4699-858c-8cb74f86fd9a"),
        //        Title = "Harry Poter",
        //        Description = "soafnsfasjfaosjfiajsiooaogahdgaod",
        //        Genre = "Fantasy",
        //        Director = "Pesho Peshev",
        //        Writer = "Gosho Goshev",
        //        CreatedOn = DateTime.Now,
        //        Runtime = TimeSpan.FromHours(2),
        //        Image = "Harrypoter.png",
        //    };

        //    var actual = await movieService.GetMovieForEdit(movieId);

        //    Assert.IsTrue(expected.Title == actual.Title &&
        //        expected.Image == actual.Image &&
        //        expected.Id == actual.Id &&
        //        expected.Director == actual.Director &&
        //        expected.Writer == actual.Writer &&
        //        expected.CreatedOn == actual.CreatedOn &&
        //        expected.Runtime == actual.Runtime &&
        //        expected.Genre == actual.Genre &&
        //        expected.Description == actual.Description);
        //}

        [Test]
        public async Task RateMoviereturnsTrue()
        {
            var movieService = serviceProvider.GetService<IMovieService>();
            string userId = "f87d224a-02cd-41cc-af7f-a1f430e2ce8e";
            Guid movieId = new Guid("ac9470b4-1e07-4699-858c-8cb74f86fd9a");

            bool isSaved = await movieService.RateMovie(userId, movieId,10.0);

            Assert.IsTrue(isSaved);
        }

        [TearDown]
        public void TearDown()
        {
            dbContext.Dispose();
        }

        private async Task SeedMovie(IApplicationDbRepository repo)
        {
            var movie = new Movie()
            {
                Id = new Guid("ac9470b4-1e07-4699-858c-8cb74f86fd9a"),
                Title = "Harry Poter",
                Description = "soafnsfasjfaosjfiajsiooaogahdgaod",
                Genre = new List<Genre>() { new Genre { Name = "Fantasy" } },
                Director = new List<Director>() { new Director { Firstname = "Pesho", Lastname = "Peshev" } },
                Writer = new List<Writer>() { new Writer { Firstname = "Gosho", Lastname = "Goshev" } },
                CreatedOn = DateTime.Now,
                Runtime = TimeSpan.FromHours(2),
                Image = "Harrypoter.png"
            };

            var user = new IdentityUser()
            {
                UserName = "Admin",
                Id = new Guid("f87d224a-02cd-41cc-af7f-a1f430e2ce8e").ToString(),
                Email = "admin@admin.com"
            };

            var movieRating = new MovieRating()
            {
                Rating = 10,
                User = user,
                UserId = new Guid("f87d224a-02cd-41cc-af7f-a1f430e2ce8e").ToString(),
                Movie = movie,
                MovieId = new Guid("ac9470b4-1e07-4699-858c-8cb74f86fd9a"),
                Id = new Guid("4eefdb92-a6ad-4903-bc42-b3919cfb92f8")
            };

            await repo.AddAsync(movie);
            await repo.AddAsync(user);
            await repo.AddAsync(movieRating);
            await repo.SaveChangesAsync();
        }
    }
}