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

        /// <summary>
        /// Retrieves a single author by ID.
        /// </summary>
        /// <param name="id">Author identifier.</param>
        /// <returns>The author if found; otherwise 404.</returns>
        [HttpGet("{id}")]
        public ActionResult<AuthorDto> GetById(int id)
        {
            var author = _authorRepository.GetById(id);
            if (author == null)
            {
                return NotFound();
            }

            return Ok(new AuthorDto { Id = author.Id, Name = author.Name });
        }

        /// <summary>
        /// Creates a new author.
        /// </summary>
        /// <param name="authorDto">The DTO containing author data.</param>
        /// <returns>The created author.</returns>
        [HttpPost]
        public ActionResult<AuthorDto> Create([FromBody] CreateAuthorDto authorDto)
        {
            if (authorDto == null || string.IsNullOrWhiteSpace(authorDto.Name))
            {
                return BadRequest("Invalid author data.");
            }

            var createdAuthor = _authorRepository.Add(new Author
            {
                Name = authorDto.Name.Trim()
            });

            var result = new AuthorDto { Id = createdAuthor.Id, Name = createdAuthor.Name };
            return CreatedAtAction(nameof(GetById), new { id = createdAuthor.Id }, result);
        }

        /// <summary>
        /// Deletes an author by ID.
        /// </summary>
        /// <param name="id">Author identifier.</param>
        /// <returns>204 No Content on success, 400 Bad Request if books exist, or 404 Not Found if missing.</returns>
        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            var existingAuthor = _authorRepository.GetById(id);
            if (existingAuthor == null)
            {
                return NotFound();
            }

            if (_authorRepository.HasBooks(id))
            {
                return BadRequest("Cannot delete an author with associated books.");
            }

            _authorRepository.Remove(id);
            return NoContent();
        }
    }
}
