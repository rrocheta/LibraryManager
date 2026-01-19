using LibraryManager.API.Models;

namespace LibraryManager.API.Repositories
{
    /// <summary>
    /// Repository implementation for managing Author entities.
    /// Provides data access methods for retrieving authors from the database.
    /// </summary>
    public class AuthorRepository : IAuthorRepository
    {
        private readonly AppDbContext _dbContext;

        public AuthorRepository(AppDbContext appDbContext)
        {
            _dbContext = appDbContext;
        }

        /// <summary>
        /// Retrieves all authors from the database.
        /// </summary>
        /// <param name="name">Optional filter by author name (currently unused).</param>
        /// <returns>A collection of <see cref="Author"/> entities.</returns>
        public IEnumerable<Author> GetAll(string? name = null)
        {
            return _dbContext.Authors.ToList();
        }

        /// <summary>
        /// Retrieves a single author by its ID.
        /// </summary>
        /// <param name="id">Author identifier.</param>
        /// <returns>The matching <see cref="Author"/> or null.</returns>
        public Author? GetById(int id)
        {
            return _dbContext.Authors.Find(id);
        }

        /// <summary>
        /// Adds a new author to the database.
        /// </summary>
        /// <param name="author">The author entity.</param>
        /// <returns>The created <see cref="Author"/>.</returns>
        public Author Add(Author author)
        {
            _dbContext.Authors.Add(author);
            _dbContext.SaveChanges();
            return author;
        }
    }
}
