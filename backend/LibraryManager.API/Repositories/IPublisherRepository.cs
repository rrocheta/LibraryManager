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
    }
}
