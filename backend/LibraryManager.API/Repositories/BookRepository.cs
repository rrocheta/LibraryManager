using LibraryManager.API.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryManager.API.Repositories
{
    /// <summary>
    /// Repository implementation for managing <see cref="Book"/> entities.
    /// Provides methods for retrieving, creating, updating, and deleting books in the database.
    /// </summary>
    public class BookRepository : IBookRepository
    {

        private readonly AppDbContext _dbContext;

        public BookRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// Retrieves all books, optionally filtering by title, author, or borrowed status.
        /// </summary>
        /// <param name="title">Optional filter for book title.</param>
        /// <param name="authorId">Optional filter for author ID.</param>
        /// <param name="isBorrowed">Optional filter for borrowed status.</param>
        /// <returns>A collection of <see cref="Book"/> entities.</returns>
        public IEnumerable<Book> GetAll(string? title = null, int? authorId = null, bool? isBorrowed = null)
        {
            var query = _dbContext.Books
                .Include(b => b.Author)
                .Include(b => b.Publisher)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(title))
                query = query.Where(b => b.Title.Contains(title));

            if (authorId.HasValue)
                query = query.Where(b => b.AuthorId == authorId.Value);

            if (isBorrowed.HasValue)
                query = query.Where(b => b.IsBorrowed == isBorrowed.Value);

            return query.ToList();
        }

        /// <summary>
        /// Retrieves a single book by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the book.</param>
        /// <returns>The matching <see cref="Book"/> entity, or null if not found.</returns>
        public Book? GetById(Guid id)
        {
            return _dbContext.Books
                .Include(b => b.Author)
                .Include(b => b.Publisher)
                .FirstOrDefault(b => b.Id == id);
        }

        /// <summary>
        /// Adds a new book to the database.
        /// </summary>
        /// <param name="book">The <see cref="Book"/> entity to add.</param>
        /// <returns>The created <see cref="Book"/> entity with its assigned ID.</returns>
        public Book Add(Book book)
        {
            if (book.Id == Guid.Empty)
                book.Id = Guid.NewGuid();
            _dbContext.Books.Add(book);
            _dbContext.SaveChanges();

            return book;
        }

        /// <summary>
        /// Updates an existing book's details in the database.
        /// </summary>
        /// <param name="book">The <see cref="Book"/> entity with updated data.</param>
        public void Update(Book book)
        {
            var existingBook = _dbContext.Books.Find(book.Id);
            if (existingBook != null)
            {
                existingBook.Title = book.Title;
                existingBook.AuthorId = book.AuthorId;
                existingBook.PublisherId = book.PublisherId;
                existingBook.IsBorrowed = book.IsBorrowed;
                _dbContext.SaveChanges();
            }
        }

        /// <summary>
        /// Removes a book from the database by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the book to remove.</param>
        public void Remove(Guid id)
        {
            var book = _dbContext.Books.Find(id);
            if (book != null)
            {
                _dbContext.Books.Remove(book);
                _dbContext.SaveChanges();
            }
        }

        /// <summary>
        /// Clears all books from the database.  
        /// Intended for testing purposes only.
        /// </summary>
        public void Clear()
        {
            _dbContext.Books.RemoveRange(_dbContext.Books);
            _dbContext.SaveChanges();
        }
    }
}
