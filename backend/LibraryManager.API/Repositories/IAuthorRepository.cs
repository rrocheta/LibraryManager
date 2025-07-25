using LibraryManager.API.Models;

namespace LibraryManager.API.Repositories
{

    public interface IAuthorRepository
    {
        IEnumerable<Author> GetAll(string? name = null);
    }

}
