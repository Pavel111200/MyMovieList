using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace MyMovieList.Infrastructure.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserRating>()
                .Property<Guid>("MovieId")
                .HasDefaultValue(new Guid("00000000-0000-0000-0000-000000000000"));

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Movie> Movies { get; set; }
        public DbSet<Writer> Writers { get; set; }
        public DbSet<Director> Directors { get; set; }
        public DbSet<TVShow> TVShows { get; set; }
        public DbSet<Genre> Genre { get; set; }
        public DbSet<UserSuggestion> UserSuggestions { get; set; }
        public DbSet<UserRating> UserRatings { get; set; }

        //public DbSet<WriterMovie> WriterMovies { get; set; }
        //public DbSet<DirectorMovie> DirectorMovies { get; set; }
        //public DbSet<WriterTVShow> WriterTVShows { get; set; }
        //public DbSet<GenreTVShow> GenreTVShows { get; set; }
        //public DbSet<GenreMovie> GenreMovies { get; set; }
    }
}