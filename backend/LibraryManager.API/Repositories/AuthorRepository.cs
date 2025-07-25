using LibraryManager.API.Models;

namespace LibraryManager.API.Repositories
{
    public class AuthorRepository : IAuthorRepository
    {
        private readonly AppDbContext _dbContext;

        public AuthorRepository(AppDbContext appDbContext)
        {
            _dbContext = appDbContext;
        }

        public IEnumerable<Author> GetAll(string? name = null)
        {
            return _dbContext.Authors.ToList();
        }
    }
}
