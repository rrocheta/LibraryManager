using LibraryManager.API.Models;

namespace LibraryManager.API.Repositories
{
    public interface IBookRepository
    {
        IEnumerable<Book> GetAll(string? title = null, int? authorId = null);
        Book? GetById(Guid id);
        Book Add(Book book);
        void Update(Book book);
        void Remove(Guid id);
    }
}
