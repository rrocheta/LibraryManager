using LibraryManager.API.Models;

namespace LibraryManager.API.Repositories
{
    public class BookRepository : IBookRepository
    {
        private static readonly List<Book> _books = new();

        public IEnumerable<Book> GetAll(string? title = null, int? authorId = null)
        {
            return _books.Where(b =>
                (string.IsNullOrWhiteSpace(title) || b.Title.Contains(title, StringComparison.OrdinalIgnoreCase)) &&
                (!authorId.HasValue || b.AuthorId == authorId.Value));
        }

        public Book? GetById(Guid id)
        {
            return _books.FirstOrDefault(b => b.Id == id);
        }

        public void Add(Book book)
        {
            _books.Add(book);
        }

        public void Update(Book book)
        {
            var index = _books.FindIndex(b => b.Id == book.Id);
            if (index >= 0)
            {
                _books[index] = book;
            }
        }

        public void Remove(Guid id)
        {
            var book = GetById(id);
            if (book != null)
            {
                _books.Remove(book);
            }
        }

        // For testing purposes, clear the repository
        public void Clear() => _books.Clear();
    }
}
