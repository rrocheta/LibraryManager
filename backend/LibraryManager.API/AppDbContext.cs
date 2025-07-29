using LibraryManager.API.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryManager.API
{
    /// <summary>
    /// Database context for the LibraryManager application.
    /// Manages entities: Authors, Books, and Publishers.
    /// </summary>
    public class AppDbContext : DbContext
    {
        public DbSet<Author> Authors { get; set; }

        public DbSet<Book> Books { get; set; }

        public DbSet<Publisher> Publishers { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        /// <summary>
        /// Configures the model and seeds initial data for Authors and Publishers.
        /// </summary>
        /// <param name="modelBuilder">The builder used to construct the model for the context.</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Author>().HasData(
                new Author { Id = 1, Name = "Fernando Pessoa" },
                new Author { Id = 2, Name = "Hernandes Dias Lopes" },
                new Author { Id = 3, Name = "Reinhard Bonnke" }
            );

            modelBuilder.Entity<Publisher>().HasData(
                new Publisher { Id = 1, Name = "Porto Editora" },
                new Publisher { Id = 2, Name = "Hagnos" },
                new Publisher { Id = 3, Name = "CPAD" }
            );
        }
    }
}
