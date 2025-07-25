using LibraryManager.API.Models;

namespace LibraryManager.API.Repositories
{
    public class PublisherRepository : IPublisherRepository
    {
        private readonly AppDbContext _dbContext;

        public PublisherRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IEnumerable<Publisher> GetAll(string? name = null)
        {
            return _dbContext.Publishers.ToList();
        }
    }
}
