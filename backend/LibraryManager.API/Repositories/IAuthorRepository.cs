using LibraryManager.API.Models;

namespace LibraryManager.API.Repositories
{
    /// <summary>
    /// Interface for accessing and managing <see cref="Author"/> entities.
    /// </summary>
    public interface IAuthorRepository
    {
        /// <summary>
        /// Retrieves all authors, optionally filtered by name.
        /// </summary>
        /// <param name="name">Optional filter for author name.</param>
        /// <returns>A collection of <see cref="Author"/> entities.</returns>
        IEnumerable<Author> GetAll(string? name = null);
    }

}
