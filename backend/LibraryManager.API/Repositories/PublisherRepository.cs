using LibraryManager.API.Models;

namespace LibraryManager.API.Repositories
{
    /// <summary>
    /// Provides data access operations for <see cref="Publisher"/> entities.
    /// </summary>
    public class PublisherRepository : IPublisherRepository
    {
        private readonly AppDbContext _dbContext;

        public PublisherRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// Retrieves all publishers from the database.
        /// </summary>
        /// <param name="name">Optional filter by publisher name (currently unused).</param>
        /// <returns>A collection of <see cref="Publisher"/> entities.</returns>
        public IEnumerable<Publisher> GetAll(string? name = null)
        {
            return _dbContext.Publishers.ToList();
        }
    }
}
