using LibraryManager.API.Dtos;
using LibraryManager.API.Models;
using LibraryManager.API.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManager.API.Controllers
{
    /// <summary>
    /// Controller for managing book-related API endpoints.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class BooksController : ControllerBase
    {
        private readonly IBookRepository _repository;

        public BooksController(IBookRepository repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// Retrieves a filtered list of books.
        /// </summary>
        /// <param name="title">Optional title filter to search books by title.</param>
        /// <param name="authorId">Optional author ID filter to search books by author.</param>
        /// <param name="isBorrowed">Optional borrowed status filter to search books by their borrowing status.</param>
        /// <returns>A list of books matching the filters, mapped to <see cref="BookDto"/>.</returns>
        [HttpGet]
        public ActionResult<IEnumerable<BookDto>> GetAll([FromQuery] string? title, [FromQuery] int? authorId, [FromQuery] bool? isBorrowed)
        {
            var books = _repository.GetAll(title, authorId, isBorrowed);

            var result = books.Select(book => new BookDto
            {
                Id = book.Id,
                Title = book.Title,
                Author = new AuthorDto { Id = book.Author.Id, Name = book.Author.Name },
                Publisher = new PublisherDto { Id = book.Publisher.Id, Name = book.Publisher.Name },
                IsBorrowed = book.IsBorrowed
            });

            return Ok(result);
        }

        /// <summary>
        /// Retrieves a single book by its unique identifier.
        /// </summary>
        /// <param name="id">The GUID identifier of the book.</param>
        /// <returns>The book mapped to <see cref="BookDto"/> if found; otherwise, 404 Not Found or 400 Bad Request.</returns>
        [HttpGet("{id}")]
        public ActionResult<BookDto> GetById(Guid id)
        {
            if (id == Guid.Empty)
                return BadRequest();

            var book = _repository.GetById(id);
            if (book == null)
                return NotFound();

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

        /// <summary>
        /// Creates a new book record.
        /// </summary>
        /// <param name="bookDto">The DTO containing data needed to create a book.</param>
        /// <returns>The created book mapped to <see cref="BookDto"/> with 201 Created status or 400 Bad Request on invalid data.</returns>
        [HttpPost]
        public ActionResult<BookDto> Create([FromBody] CreateBookDto bookDto)
        {
            if (bookDto == null || string.IsNullOrWhiteSpace(bookDto.Title))
            {
                return BadRequest("Invalid book data.");
            }

            var book = _repository.GetAll(bookDto.Title, bookDto.AuthorId, null);

            if (book != null)
            {
                return BadRequest("A book with the same title already exists for this editor.");
            }

            var createdBook = _repository.Add(new Book
            {
                Id = Guid.NewGuid(),
                Title = bookDto.Title,
                AuthorId = bookDto.AuthorId,
                PublisherId = bookDto.PublisherId,
                IsBorrowed = false
            });

            var bookWithDetails = _repository.GetById(createdBook.Id);

            if (bookWithDetails == null)
            {
                return NotFound();
            }

            var author = bookWithDetails.Author ?? new Author { Id = 0, Name = "Unknown" };
            var publisher = bookWithDetails.Publisher ?? new Publisher { Id = 0, Name = "Unknown" };

            var mappedBookDto = new BookDto
            {
                Id = bookWithDetails.Id,
                Title = bookWithDetails.Title,
                Author = new AuthorDto { Id = author.Id, Name = author.Name },
                Publisher = new PublisherDto { Id = publisher.Id, Name = publisher.Name },
                IsBorrowed = bookWithDetails.IsBorrowed
            };

            return CreatedAtAction(nameof(GetById), new { id = bookWithDetails.Id }, mappedBookDto);
        }

        /// <summary>
        /// Updates an existing book identified by ID.
        /// </summary>
        /// <param name="id">The GUID of the book to update.</param>
        /// <param name="bookDto">The DTO containing updated book data.</param>
        /// <returns>204 No Content on success, 400 Bad Request on invalid data or if the book is borrowed, or 404 Not Found if not found.</returns>
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

        /// <summary>
        /// Deletes a book by its ID.
        /// </summary>
        /// <param name="id">The GUID identifier of the book to delete.</param>
        /// <returns>204 No Content on success, 400 Bad Request if the book is borrowed, or 404 Not Found if the book doesn't exist.</returns>
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

        /// <summary>
        /// Marks a book as borrowed.
        /// </summary>
        /// <param name="id">The GUID identifier of the book to borrow.</param>
        /// <returns>204 No Content on success, 400 Bad Request if already borrowed, or 404 Not Found if book doesn't exist.</returns>
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

        /// <summary>
        /// Marks a book as returned (not borrowed).
        /// </summary>
        /// <param name="id">The GUID identifier of the book to return.</param>
        /// <returns>204 No Content on success, 400 Bad Request if book is not borrowed, or 404 Not Found if book doesn't exist.</returns>
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

