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
        /// Configures the model and seeds initial data for Authors, Publishers, and Books.
        /// </summary>
        /// <param name="modelBuilder">The builder used to construct the model for the context.</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Author>().HasData(
                new Author { Id = 1, Name = "Fernando Pessoa" },
                new Author { Id = 2, Name = "Hernandes Dias Lopes" },
                new Author { Id = 3, Name = "Reinhard Bonnke" },
                new Author { Id = 4, Name = "Jane Austen" },
                new Author { Id = 5, Name = "George Orwell" }
            );

            modelBuilder.Entity<Publisher>().HasData(
                new Publisher { Id = 1, Name = "Porto Editora" },
                new Publisher { Id = 2, Name = "Hagnos" },
                new Publisher { Id = 3, Name = "CPAD" },
                new Publisher { Id = 4, Name = "Penguin Classics" }
            );

            modelBuilder.Entity<Book>().HasData(
                new Book
                {
                    Id = Guid.Parse("b1d8b2dd-2d77-4b42-9dc6-7f4c1ec4c001"),
                    Title = "The Book of Disquiet",
                    AuthorId = 1,
                    PublisherId = 1,
                    IsBorrowed = false
                },
                new Book
                {
                    Id = Guid.Parse("b1d8b2dd-2d77-4b42-9dc6-7f4c1ec4c002"),
                    Title = "Message",
                    AuthorId = 1,
                    PublisherId = 1,
                    IsBorrowed = false
                },
                new Book
                {
                    Id = Guid.Parse("b1d8b2dd-2d77-4b42-9dc6-7f4c1ec4c003"),
                    Title = "Poesias Ineditas",
                    AuthorId = 1,
                    PublisherId = 1,
                    IsBorrowed = false
                },
                new Book
                {
                    Id = Guid.Parse("b1d8b2dd-2d77-4b42-9dc6-7f4c1ec4c004"),
                    Title = "O Deus que Destrona Deuses",
                    AuthorId = 2,
                    PublisherId = 2,
                    IsBorrowed = false
                },
                new Book
                {
                    Id = Guid.Parse("b1d8b2dd-2d77-4b42-9dc6-7f4c1ec4c005"),
                    Title = "Jesus Cristo: O Deus Filho",
                    AuthorId = 2,
                    PublisherId = 2,
                    IsBorrowed = false
                },
                new Book
                {
                    Id = Guid.Parse("b1d8b2dd-2d77-4b42-9dc6-7f4c1ec4c006"),
                    Title = "Avivamento",
                    AuthorId = 3,
                    PublisherId = 3,
                    IsBorrowed = false
                },
                new Book
                {
                    Id = Guid.Parse("b1d8b2dd-2d77-4b42-9dc6-7f4c1ec4c007"),
                    Title = "Living a Life of Fire",
                    AuthorId = 3,
                    PublisherId = 3,
                    IsBorrowed = false
                },
                new Book
                {
                    Id = Guid.Parse("b1d8b2dd-2d77-4b42-9dc6-7f4c1ec4c008"),
                    Title = "Pride and Prejudice",
                    AuthorId = 4,
                    PublisherId = 4,
                    IsBorrowed = false
                },
                new Book
                {
                    Id = Guid.Parse("b1d8b2dd-2d77-4b42-9dc6-7f4c1ec4c009"),
                    Title = "Sense and Sensibility",
                    AuthorId = 4,
                    PublisherId = 4,
                    IsBorrowed = false
                },
                new Book
                {
                    Id = Guid.Parse("b1d8b2dd-2d77-4b42-9dc6-7f4c1ec4c00a"),
                    Title = "Emma",
                    AuthorId = 4,
                    PublisherId = 4,
                    IsBorrowed = false
                },
                new Book
                {
                    Id = Guid.Parse("b1d8b2dd-2d77-4b42-9dc6-7f4c1ec4c00b"),
                    Title = "Persuasion",
                    AuthorId = 4,
                    PublisherId = 4,
                    IsBorrowed = false
                },
                new Book
                {
                    Id = Guid.Parse("b1d8b2dd-2d77-4b42-9dc6-7f4c1ec4c00c"),
                    Title = "1984",
                    AuthorId = 5,
                    PublisherId = 4,
                    IsBorrowed = false
                },
                new Book
                {
                    Id = Guid.Parse("b1d8b2dd-2d77-4b42-9dc6-7f4c1ec4c00d"),
                    Title = "Animal Farm",
                    AuthorId = 5,
                    PublisherId = 4,
                    IsBorrowed = false
                },
                new Book
                {
                    Id = Guid.Parse("b1d8b2dd-2d77-4b42-9dc6-7f4c1ec4c00e"),
                    Title = "Homage to Catalonia",
                    AuthorId = 5,
                    PublisherId = 4,
                    IsBorrowed = false
                },
                new Book
                {
                    Id = Guid.Parse("b1d8b2dd-2d77-4b42-9dc6-7f4c1ec4c00f"),
                    Title = "A Clergyman's Daughter",
                    AuthorId = 5,
                    PublisherId = 4,
                    IsBorrowed = false
                }
            );
        }
    }
}
