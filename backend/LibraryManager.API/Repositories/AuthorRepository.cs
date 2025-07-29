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
    }
}
