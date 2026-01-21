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

        /// <summary>
        /// Retrieves a single publisher by its ID.
        /// </summary>
        /// <param name="id">Publisher identifier.</param>
        /// <returns>The matching <see cref="Publisher"/> or null.</returns>
        public Publisher? GetById(int id)
        {
            return _dbContext.Publishers.Find(id);
        }

        /// <summary>
        /// Adds a new publisher to the database.
        /// </summary>
        /// <param name="publisher">The publisher entity.</param>
        /// <returns>The created <see cref="Publisher"/>.</returns>
        public Publisher Add(Publisher publisher)
        {
            _dbContext.Publishers.Add(publisher);
            _dbContext.SaveChanges();
            return publisher;
        }

        /// <summary>
        /// Checks whether a publisher has any associated books.
        /// </summary>
        /// <param name="id">Publisher identifier.</param>
        /// <returns>True when at least one book is linked to the publisher.</returns>
        public bool HasBooks(int id)
        {
            return _dbContext.Books.Any(book => book.PublisherId == id);
        }

        /// <summary>
        /// Removes a publisher from the database by its identifier.
        /// </summary>
        /// <param name="id">Publisher identifier.</param>
        public void Remove(int id)
        {
            var publisher = _dbContext.Publishers.Find(id);
            if (publisher != null)
            {
                _dbContext.Publishers.Remove(publisher);
                _dbContext.SaveChanges();
            }
        }
    }
}
