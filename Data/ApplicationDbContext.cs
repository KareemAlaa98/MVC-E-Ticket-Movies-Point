using Microsoft.EntityFrameworkCore;
using Movies_Point.Models;
using Movies_Point.Configurations;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Movies_Point.ViewModels;

namespace Movies_Point.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Actor> Actors { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Cinema> Cinemas { get; set; }
        public DbSet<Movie> Movies { get; set; }
        public DbSet<ActorMovie> ActorMovies { get; set; }

        public ApplicationDbContext()
        {
        }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
        {
        }
        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    base.OnConfiguring(optionsBuilder);

        //    var builder = new ConfigurationBuilder().AddJsonFile("appsettings.json", true, true)
        //        .Build().GetConnectionString("DefaultPath");

        //    optionsBuilder.UseSqlServer(builder);
        //}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            new MovieEntityTypeConfiguration().Configure(modelBuilder.Entity<Movie>());
            new ActorEntityTypeConfiguration().Configure(modelBuilder.Entity<Actor>());
            new CategoryEntityTypeConfiguration().Configure(modelBuilder.Entity<Category>());
            new CinemaEntityTypeConfiguration().Configure(modelBuilder.Entity<Cinema>());
            new ActorMovieEntityTypeConfiguration().Configure(modelBuilder.Entity<ActorMovie>());
        }
        public DbSet<Movies_Point.ViewModels.ApplicationUserVM> ApplicationUserVM { get; set; } = default!;
        public DbSet<Movies_Point.ViewModels.UserLoginVM> UserLoginVM { get; set; } = default!;
        public DbSet<Movies_Point.ViewModels.UserRoleVM> UserRoleVM { get; set; } = default!;
        public DbSet<Movies_Point.ViewModels.CinemaViewModel> CinemaViewModel { get; set; } = default!;
        public DbSet<Movies_Point.ViewModels.ActorViewModel> ActorViewModel { get; set; } = default!;
    }
}
