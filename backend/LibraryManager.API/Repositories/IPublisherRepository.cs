using LibraryManager.API.Models;

namespace LibraryManager.API.Repositories
{
    public interface IPublisherRepository
    {
        IEnumerable<Publisher> GetAll(string? name = null);
    }
}
