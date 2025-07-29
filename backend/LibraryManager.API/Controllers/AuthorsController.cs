using LibraryManager.API.Dtos;
using LibraryManager.API.Models;
using LibraryManager.API.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManager.API.Controllers
{
    /// <summary>
    /// Controller for author-related operations.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class AuthorsController : Controller
    {
        private readonly IAuthorRepository _authorRepository;

        public AuthorsController(IAuthorRepository authorRepository)
        {
            _authorRepository = authorRepository;
        }

        /// <summary>
        /// Retrieves all authors.
        /// </summary>
        /// <param name="name">Optional parameter to filter authors by name (currently unused).</param>
        /// <returns>Returns a list of authors as <see cref="AuthorDto"/> or 404 if none found.</returns>
        [HttpGet]
        public ActionResult<IEnumerable<Author>> GetAll(string? name = null)
        {
            var authors = _authorRepository.GetAll();
            if (authors == null || !authors.Any())
            {
                return NotFound("No authors found.");
            }
            var result = authors.Select(aut => new AuthorDto { Id = aut.Id, Name = aut.Name });
            return Ok(result);
        }
    }
}
