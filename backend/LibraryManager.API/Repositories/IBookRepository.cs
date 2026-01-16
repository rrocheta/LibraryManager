using LibraryManager.API.Models;

namespace LibraryManager.API.Repositories
{
    /// <summary>
    /// Defines the contract for book repository operations.
    /// </summary>
    public interface IBookRepository
    {
        /// <summary>
        /// Retrieves all books, optionally filtered by title, author, or borrowed status.
        /// </summary>
        /// <param name="title">Optional filter by book title.</param>
        /// <param name="authorId">Optional filter by author ID.</param>
        /// <param name="isBorrowed">Optional filter by borrowed status.</param>
        /// <returns>A collection of <see cref="Book"/> entities.</returns>
        IEnumerable<Book> GetAll(string? title = null, int? authorId = null, bool? isBorrowed = null);

        /// <summary>
        /// Retrieves a paged set of books, optionally filtered by title, author, or borrowed status.
        /// </summary>
        /// <param name="title">Optional filter by book title.</param>
        /// <param name="authorId">Optional filter by author ID.</param>
        /// <param name="isBorrowed">Optional filter by borrowed status.</param>
        /// <param name="page">Page number (1-based).</param>
        /// <param name="pageSize">Number of items per page.</param>
        /// <param name="totalCount">Total count of items matching the filters.</param>
        /// <returns>A paged collection of <see cref="Book"/> entities.</returns>
        IEnumerable<Book> GetPaged(
            string? title,
            int? authorId,
            bool? isBorrowed,
            int page,
            int pageSize,
            out int totalCount);

        /// <summary>
        /// Retrieves a book by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the book.</param>
        /// <returns>The <see cref="Book"/> entity, or null if not found.</returns>
        /// 
        Book? GetById(Guid id);

        /// <summary>
        /// Adds a new book to the repository.
        /// </summary>
        /// <param name="book">The book entity to add.</param>
        /// <returns>The added <see cref="Book"/> entity.</returns>
        Book Add(Book book);

        /// <summary>
        /// Updates an existing book in the repository.
        /// </summary>
        /// <param name="book">The updated book entity.</param>
        void Update(Book book);

        /// <summary>
        /// Removes a book from the repository by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the book to remove.</param>
        void Remove(Guid id);
    }
}
