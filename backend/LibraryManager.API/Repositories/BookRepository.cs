using LibraryManager.API.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryManager.API.Repositories
{
    public class BookRepository : IBookRepository
    {

        private readonly AppDbContext _dbContext;

        public BookRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IEnumerable<Book> GetAll(string? title = null, int? authorId = null)
        {
            var query = _dbContext.Books
                .Include(b => b.Author)
                .Include(b => b.Publisher)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(title))
                query = query.Where(b => b.Title.Contains(title));

            if (authorId.HasValue)
                query = query.Where(b => b.AuthorId == authorId.Value);

            return query.ToList();
        }

        public Book? GetById(Guid id)
        {
            return _dbContext.Books
                .Include(b => b.Author)
                .Include(b => b.Publisher)
                .FirstOrDefault(b => b.Id == id);
        }

        public Book Add(Book book)
        {
            if (book.Id == Guid.Empty)
                book.Id = Guid.NewGuid();
            _dbContext.Books.Add(book);
            _dbContext.SaveChanges();

            return book;
        }

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

        public void Remove(Guid id)
        {
            var book = _dbContext.Books.Find(id);
            if (book != null)
            {
                _dbContext.Books.Remove(book);
                _dbContext.SaveChanges();
            }
        }

        // For testing purposes, clear the repository
        public void Clear()
        {
            _dbContext.Books.RemoveRange(_dbContext.Books);
            _dbContext.SaveChanges();
        }
    }
}
