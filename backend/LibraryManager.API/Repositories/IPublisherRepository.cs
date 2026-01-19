using LibraryManager.API.Models;

namespace LibraryManager.API.Repositories
{
    /// <summary>
    /// Defines the contract for publisher repository operations.
    /// </summary>
    public interface IPublisherRepository
    {
        /// <summary>
        /// Retrieves all publishers, optionally filtered by name.
        /// </summary>
        /// <param name="name">Optional filter by publisher name.</param>
        /// <returns>A collection of <see cref="Publisher"/> entities.</returns>
        IEnumerable<Publisher> GetAll(string? name = null);

        /// <summary>
        /// Retrieves a publisher by its ID.
        /// </summary>
        /// <param name="id">Publisher identifier.</param>
        /// <returns>The matching <see cref="Publisher"/> or null.</returns>
        Publisher? GetById(int id);

        /// <summary>
        /// Adds a new publisher to the database.
        /// </summary>
        /// <param name="publisher">The publisher to add.</param>
        /// <returns>The created <see cref="Publisher"/>.</returns>
        Publisher Add(Publisher publisher);
    }
}
