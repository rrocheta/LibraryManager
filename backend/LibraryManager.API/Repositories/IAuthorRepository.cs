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

        /// <summary>
        /// Retrieves an author by its ID.
        /// </summary>
        /// <param name="id">Author identifier.</param>
        /// <returns>The matching <see cref="Author"/> or null.</returns>
        Author? GetById(int id);

        /// <summary>
        /// Adds a new author to the database.
        /// </summary>
        /// <param name="author">The author to add.</param>
        /// <returns>The created <see cref="Author"/>.</returns>
        Author Add(Author author);
    }

}
