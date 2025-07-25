using LibraryManager.API.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryManager.API
{
    public class AppDbContext : DbContext
    {
        public DbSet<Author> Authors { get; set; }

        public DbSet<Book> Books { get; set; }

        public DbSet<Publisher> Publishers { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseInMemoryDatabase("LibraryDb");
        //}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Seed data for Authors
            modelBuilder.Entity<Author>().HasData(
                new Author { Id = 1, Name = "Fernando Pessoa" },
                new Author { Id = 2, Name = "Hernandes Dias Lopes" },
                new Author { Id = 3, Name = "Reinhard Bonnke" }
            );
            // Seed data for Publishers
            modelBuilder.Entity<Publisher>().HasData(
                new Publisher { Id = 1, Name = "Porto Editora" },
                new Publisher { Id = 2, Name = "Hagnos" },
                new Publisher { Id = 3, Name = "CPAD" }
            );
        }
    }
}
