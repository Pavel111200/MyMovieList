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
using System.Text;
using System.Threading.Tasks;

namespace MyMovieList.Test
{
    public class TVShowServiceTest
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
                .AddSingleton<ITVShowService,TVShowService>()
                .BuildServiceProvider();

            var repo = serviceProvider.GetService<IApplicationDbRepository>();
            await SeedMovie(repo);
        }

        [Test]
        public async Task AddTVShowReturnsTrueIfValid()
        {
            var showService = serviceProvider.GetService<ITVShowService>();

            var validShow = new AddTVShowViewModel()
            {
                Title = "Harry Poter",
                Description = "soafnsfasjfaosjfiajsiooaogahdgaod",
                Genre = "Fantasy",
                Writer = "Gosho Goshev",
                Image = "Harrypoter.png"
            };
            bool isSaved = await showService.AddShow(validShow);
            Assert.IsTrue(isSaved);
        }

        [Test]
        public async Task GetAllShowsReturnsTheCorrectShows()
        {
            var showService = serviceProvider.GetService<ITVShowService>();

            var expected = new AllTVShowsViewModel()
            {
                Title = "Dr. House",
                Image = "Drhouse.png",
                Id = new Guid("ab9470b4-1e07-4699-858c-8cb74f86fd9a"),
                Season = "5"
            };
            var actual = (await showService.GetAllTVShows()).First();

            Assert.IsTrue(expected.Id == actual.Id && 
                expected.Title == actual.Title &&
                expected.Image == actual.Image &&
                expected.Season == actual.Season);
        }

        [Test]
        public async Task GetGenresReturnsCorrectGenres()
        {
            var showService = serviceProvider.GetService<ITVShowService>();

            var expected = new GenreViewModel
            {
                Genre = "Drama"
            };
            var actual = (await showService.GetGenres()).First();

            Assert.AreEqual(expected.Genre, actual.Genre);
        }

        [Test]
        public async Task GetLikedShowsReturnsTheCorrectShows()
        {
            var showService = serviceProvider.GetService<ITVShowService>();
            string userId = "f87d224a-02cd-41cc-af7f-a1f430e2ce8e";

            var expected = new LikedShowsViewModel
            {
                Id = new Guid("ab9470b4-1e07-4699-858c-8cb74f86fd9a"),
                Title = "Dr. House",
                Image = "Drhouse.png",
                Season = 5,
                YourRating = 10,
                OverallRating = 1
            };
            var actual = (await showService.GetLikedShows(userId)).First();

            Assert.IsTrue(expected.Id == actual.Id &&
                expected.Title == actual.Title &&
                expected.Image == actual.Image &&
                expected.Season == actual.Season &&
                expected.YourRating == actual.YourRating &&
                expected.OverallRating == actual.OverallRating);
        }

        //[Test]
        //public async Task GetTVShowForEditReturnsCorrectTVShow()
        //{
        //    var showService = serviceProvider.GetService<ITVShowService>();
        //    string showId = "ab9470b4-1e07-4699-858c-8cb74f86fd9a";

        //    var expected = new EditTVShowViewModel
        //    {
        //        Id = new Guid("ab9470b4-1e07-4699-858c-8cb74f86fd9a"),
        //        Title = "Dr. House",
        //        Description = "Cool description",
        //        Genre = "Drama",
        //        Writer = "Gosho Goshev",
        //        Image = "Drhouse.png",
        //        NumberOfEpisodes = 20,
        //        Season = 5
        //    };
        //    var actual = await showService.GetTVShowForEdit(showId);

        //    Assert.IsTrue(expected.Title == actual.Title &&
        //        expected.Image == actual.Image &&
        //        expected.Id == actual.Id &&
        //        expected.Season == actual.Season &&
        //        expected.NumberOfEpisodes == actual.NumberOfEpisodes&&
        //        expected.Writer == actual.Writer &&
        //        expected.Genre == actual.Genre &&
        //        expected.Description == actual.Description);
        //}

        //[Test]
        //public async Task GetTVShowDetailsReturnsCorrectTVShow()
        //{
        //    var showService = serviceProvider.GetService<ITVShowService>();
        //    string showId = "ab9470b4-1e07-4699-858c-8cb74f86fd9a";

        //    var expected = new TVShowDetailsViewModel()
        //    {
        //        Id = new Guid("ab9470b4-1e07-4699-858c-8cb74f86fd9a"),
        //        Title = "Dr. House",
        //        Description = "Cool description",
        //        Genre = "Drama",
        //        Writer = "Gosho Goshev",
        //        Image = "Drhouse.png",
        //        NumberOfEpisodes = 20,
        //        Season = 5,
        //        Rating = 1,
        //    };

        //    var actual = await showService.GetTVShowDetails(showId);

        //    Assert.IsTrue(expected.Title == actual.Title &&
        //        expected.Image == actual.Image &&
        //        expected.Id == actual.Id &&
        //        expected.Writer == actual.Writer &&
        //        expected.Season == actual.Season &&
        //        expected.NumberOfEpisodes == actual.NumberOfEpisodes &&
        //        expected.Genre == actual.Genre &&
        //        expected.Description == actual.Description &&
        //        expected.Rating == actual.Rating);
        //}

        [Test]
        public async Task RateTVShowReturnsTrue()
        {
            var showService = serviceProvider.GetService<ITVShowService>();
            string userId = "f87d224a-02cd-41cc-af7f-a1f430e2ce8e";
            Guid showId = new Guid("ab9470b4-1e07-4699-858c-8cb74f86fd9a");

            bool isSaved = await showService.RateShow(userId, showId, 10.0);

            Assert.IsTrue(isSaved);
        }

        [TearDown]
        public void TearDown()
        {
            dbContext.Dispose();
        }

        private async Task SeedMovie(IApplicationDbRepository repo)
        {
            var user = new IdentityUser()
            {
                UserName = "Admin",
                Id = new Guid("f87d224a-02cd-41cc-af7f-a1f430e2ce8e").ToString(),
                Email = "admin@admin.com"
            };

            var show = new TVShow()
            {
                Id = new Guid("ab9470b4-1e07-4699-858c-8cb74f86fd9a"),
                Title = "Dr. House",
                Description = "Cool description",
                Genre = new List<Genre>() { new Genre { Name = "Drama" } },
                Writer = new List<Writer>() { new Writer { Firstname = "Gosho", Lastname = "Goshev" } },
                Image = "Drhouse.png",
                NumberOfEpisodes = 20,
                Season = 5
            };

            var showRating = new TVShowRating()
            {
                Id = new Guid("ba9470b4-1e07-4699-858c-8cb74f86fd9a"),
                User = user,
                UserId = user.Id,
                TVShow = show,
                TVShowId = show.Id,
                Rating = 10
            };

            await repo.AddAsync(user);
            await repo.AddAsync(show);
            await repo.AddAsync(showRating);
            await repo.SaveChangesAsync();
        }
    }
}
