using LibraryManager.API.Dtos;
using LibraryManager.API.Models;
using LibraryManager.API.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManager.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BooksController : ControllerBase
    {
        private readonly IBookRepository _repository;

        public BooksController(IBookRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public ActionResult<IEnumerable<BookDto>> GetAll([FromQuery] string? title, [FromQuery] int? authorId)
        {
            var books = _repository.GetAll(title, authorId);

            var result = books.Select(book => new BookDto
            {
                Id = book.Id,
                Title = book.Title,
                Author = new AuthorDto { Id = book.Author.Id, Name = book.Author.Name},
                Publisher = new PublisherDto { Id = book.Publisher.Id, Name = book.Publisher.Name},
                IsBorrowed = book.IsBorrowed
            });

            return Ok(result);
        }

        [HttpGet("{id}")]
        public ActionResult<BookDto> GetById(Guid id)
        {
            var book = _repository.GetById(id);
            if (book == null)
            {
                return NotFound();
            }
            var result = new BookDto
            {
                Id = book.Id,
                Title = book.Title,
                Author = new AuthorDto { Id = book.Author.Id, Name = book.Author.Name },
                Publisher = new PublisherDto { Id = book.Publisher.Id, Name = book.Publisher.Name },
                IsBorrowed = book.IsBorrowed
            };
            return Ok(result);
        }

        [HttpPost]
        public ActionResult<BookDto> Create([FromBody] CreateBookDto bookDto)
        {
            if (bookDto == null || string.IsNullOrWhiteSpace(bookDto.Title))
            {
                return BadRequest("Invalid book data.");
            }
            var book = new Book
            {
                Id = Guid.NewGuid(),
                Title = bookDto.Title,
                AuthorId = bookDto.AuthorId,
                PublisherId = bookDto.PublisherId,
                IsBorrowed = false
            };
            _repository.Add(book);

            return CreatedAtAction(nameof(GetById), new { id = book.Id }, bookDto);
        }

        [HttpPut("{id}")]
        public ActionResult Update(Guid id, [FromBody] BookDto bookDto)
        {
            if (bookDto == null || string.IsNullOrWhiteSpace(bookDto.Title))
            {
                return BadRequest("Invalid book data.");
            }
            var existingBook = _repository.GetById(id);
            if (existingBook == null)
            {
                return NotFound();
            }

            if (existingBook.IsBorrowed)
            {
                return BadRequest("Cannot update a borrowed book.");
            }

            existingBook.Title = bookDto.Title;
            existingBook.AuthorId = bookDto.Author.Id;
            existingBook.PublisherId = bookDto.Publisher.Id;
            _repository.Update(existingBook);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(Guid id)
        {
            var existingBook = _repository.GetById(id);
            if (existingBook == null)
            {
                return NotFound();
            }

            if (existingBook.IsBorrowed)
            {
                return BadRequest("Cannot delete a borrowed book.");
            }

            _repository.Remove(id);
            return NoContent();
        }

        [HttpPost("{id}/borrow")]
        public ActionResult Borrow(Guid id)
        {
            var book = _repository.GetById(id);
            if (book == null)
            {
                return NotFound();
            }
            if (book.IsBorrowed)
            {
                return BadRequest("Book is already borrowed.");
            }
            book.IsBorrowed = true;
            _repository.Update(book);
            return NoContent();
        }

        [HttpPost("{id}/return")]
        public ActionResult Return(Guid id)
        {
            var book = _repository.GetById(id);
            if (book == null)
            {
                return NotFound();
            }

            if (!book.IsBorrowed)
            {
                return BadRequest("Book is not borrowed.");
            }

            book.IsBorrowed = false;
            _repository.Update(book);
            return NoContent();
        }

    }
}

