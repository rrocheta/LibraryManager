using AutoFixture;
using LibraryManager.API.Controllers;
using LibraryManager.API.Dtos;
using LibraryManager.API.Models;
using LibraryManager.API.Repositories;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace LibraryManager.API.Tests.Controllers
{
    public class BooksControllerTests
    {
        private readonly Mock<IBookRepository> _mockRepository;
        private readonly BooksController _controller;
        private readonly Fixture _fixture;

        public BooksControllerTests()
        {
            _mockRepository = new Mock<IBookRepository>();
            _controller = new BooksController(_mockRepository.Object);
            _fixture = new Fixture();

            // Customize to avoid recursion (since Book references Author and Author can reference books, etc)
            _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
                .ForEach(b => _fixture.Behaviors.Remove(b));
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
        }

        [Fact]
        public void GetAll_ReturnsOk_WithListOfBooks()
        {
            // Arrange
            var book1 = _fixture.Build<Book>()
                .With(b => b.Title, "Book A")
                .With(b => b.Author, new Author { Id = 1, Name = "Author A" })
                .With(b => b.Publisher, new Publisher { Id = 1, Name = "Publisher A" })
                .Create();

            var book2 = _fixture.Build<Book>()
                .With(b => b.Title, "Book B")
                .With(b => b.Author, new Author { Id = 2, Name = "Author B" })
                .With(b => b.Publisher, new Publisher { Id = 2, Name = "Publisher B" })
                .Create();

            var books = new List<Book> { book1, book2 };
            _mockRepository.Setup(r => r.GetAll(null, null)).Returns(books);

            // Act
            var actionResult = _controller.GetAll(null, null);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var model = Assert.IsAssignableFrom<IEnumerable<BookDto>>(okResult.Value);
            var list = model.ToList();

            Assert.Equal(2, list.Count);
            Assert.Equal("Book A", list[0].Title);
            Assert.Equal("Author A", list[0].Author.Name);
            Assert.Equal("Book B", list[1].Title);
            Assert.Equal("Author B", list[1].Author.Name);
        }


        [Fact]
        public void GetAll_WithTitleOrAuthorId_ReturnsFilteredBooks()
        {
            // Arrange
            var allBooks = _fixture.Build<Book>()
                .With(b => b.Title, "Special Title")
                .With(b => b.AuthorId, 99)
                .With(b => b.Author, new Author { Id = 99, Name = "Filtered Author" })
                .CreateMany(1)
                .ToList();

            allBooks.AddRange(_fixture.Build<Book>()
                .With(b => b.Title, "Other Book")
                .With(b => b.AuthorId, 1)
                .Without(b => b.Author)
                .Without(b => b.Publisher)
                .CreateMany(2));

            _mockRepository.Setup(r => r.GetAll("Special Title", 99))
                .Returns(allBooks.Where(b => b.Title == "Special Title" && b.AuthorId == 99));

            // Act
            var result = _controller.GetAll("Special Title", 99);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var model = Assert.IsAssignableFrom<IEnumerable<BookDto>>(okResult.Value);
            var list = model.ToList();

            Assert.Single(list); // Only one book matches both filters
            Assert.Equal("Special Title", list[0].Title);
            Assert.Equal("Filtered Author", list[0].Author.Name);
        }

        [Fact]
        public void GetAll_WithNoFilters_ReturnsAllBooks()
        {
            // Arrange
            var books = _fixture.CreateMany<Book>(3).ToList();
            _mockRepository.Setup(r => r.GetAll(null, null)).Returns(books);

            // Act
            var result = _controller.GetAll(null, null);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var model = Assert.IsAssignableFrom<IEnumerable<BookDto>>(okResult.Value);
            Assert.Equal(3, model.Count());
        }

        [Fact]
        public void GetById_WithValidId_ReturnsBook()
        {
            // Arrange
            var book = _fixture.Create<Book>();
            _mockRepository.Setup(r => r.GetById(book.Id)).Returns(book);

            // Act
            var result = _controller.GetById(book.Id);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var dto = Assert.IsType<BookDto>(okResult.Value);
            Assert.Equal(book.Title, dto.Title);
            Assert.Equal(book.Author.Name, dto.Author.Name);
        }

        [Fact]
        public void GetById_WithNonExistentId_ReturnsNotFound()
        {
            // Arrange
            var id = Guid.NewGuid();
            _mockRepository.Setup(r => r.GetById(id)).Returns((Book?)null);

            // Act
            var result = _controller.GetById(id);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public void GetById_WithEmptyGuid_ReturnsBadRequest()
        {
            // Act
            var result = _controller.GetById(Guid.Empty);

            // Assert
            Assert.IsType<BadRequestResult>(result.Result);
        }

        [Fact]
        public void Create_WithValidBook_ReturnsCreatedBook()
        {
            // Arrange
            var bookDto = _fixture.Create<CreateBookDto>();

            var book = _fixture.Create<Book>();

            _mockRepository.Setup(x => x.Add(It.IsAny<Book>())).Returns(book);

            // Act
            var result = _controller.Create(bookDto);

            // Assert
            var createdResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            var createdBook = Assert.IsType<BookDto>(createdResult.Value);
            Assert.Equal(book.Title, createdBook.Title);
        }

        [Fact]
        public void Create_WithInvalidBook_ReturnsBadRequest()
        {
            // Arrange
            var bookDto = new CreateBookDto();

            // Act
            var result = _controller.Create(bookDto);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal("Invalid book data.", badRequestResult.Value);
        }

        [Fact]
        public void Create_WithEmptyTitle_ReturnsBadRequest()
        {
            // Arrange
            var bookDto = new CreateBookDto { Title = string.Empty };

            // Act
            var result = _controller.Create(bookDto);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal("Invalid book data.", badRequestResult.Value);
        }

        [Fact]
        public void Update_WithValidBook_ReturnsNoContent()
        {
            // Arrange
            var bookDto = _fixture.Create<BookDto>();
            bookDto.Id = Guid.NewGuid();
            var book = _fixture.Create<Book>();
            book.Id = bookDto.Id;

            _mockRepository.Setup(x => x.GetById(bookDto.Id)).Returns(book);
            _mockRepository.Setup(x => x.Update(It.IsAny<Book>()));

            // Act
            var result = _controller.Update(bookDto.Id, bookDto);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public void Update_WithNonExistentId_ReturnsNotFound()
        {
            // Arrange
            var bookDto = _fixture.Create<BookDto>();
            bookDto.Id = Guid.NewGuid();
            _mockRepository.Setup(x => x.GetById(bookDto.Id)).Returns((Book?)null);

            // Act
            var result = _controller.Update(bookDto.Id, bookDto);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void Update_CantUpdateIfBorrowed_ReturnsBadRequest()
        {
            // Arrange
            var bookDto = _fixture.Create<BookDto>();
            bookDto.Id = Guid.NewGuid();

            var borrowedBook = new Book
            {
                Id = bookDto.Id,
                IsBorrowed = true
            };

            _mockRepository.Setup(x => x.GetById(bookDto.Id)).Returns(borrowedBook);

            // Act
            var result = _controller.Update(bookDto.Id, bookDto);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Cannot update a borrowed book.", badRequestResult.Value);
        }

        [Fact]
        public void Delete_WithValidId_ReturnsNoContent()
        {
            // Arrange
            var id = Guid.NewGuid();
            _mockRepository.Setup(x => x.GetById(id)).Returns(new Book { Id = id });
            _mockRepository.Setup(x => x.Remove(id));

            // Act
            var result = _controller.Delete(id);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public void Delete_WithNonExistentId_ReturnsNotFound()
        {
            // Arrange
            var id = Guid.NewGuid();
            _mockRepository.Setup(x => x.GetById(id)).Returns((Book?)null);

            // Act
            var result = _controller.Delete(id);
            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void Delete_CantDeleteIfBorrowed_ReturnsBadRequest()
        {
            // Arrange
            var id = Guid.NewGuid();
            var borrowedBook = new Book
            {
                Id = id,
                IsBorrowed = true
            };

            _mockRepository.Setup(x => x.GetById(id)).Returns(borrowedBook);

            // Act
            var result = _controller.Delete(id);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Cannot delete a borrowed book.", badRequestResult.Value);
        }

        [Fact]
        public void Borrow_WithValidId_ReturnsNoContent()
        {
            // Arrange
            var id = Guid.NewGuid();
            var book = new Book { Id = id, IsBorrowed = false };
            _mockRepository.Setup(x => x.GetById(id)).Returns(book);
            _mockRepository.Setup(x => x.Update(It.IsAny<Book>()));

            // Act
            var result = _controller.Borrow(id);

            // Assert
            Assert.IsType<NoContentResult>(result);
            Assert.True(book.IsBorrowed);
        }

        [Fact]
        public void Borrow_WithNonExistentId_ReturnsNotFound()
        {
            // Arrange
            var id = Guid.NewGuid();
            _mockRepository.Setup(x => x.GetById(id)).Returns((Book?)null);

            // Act
            var result = _controller.Borrow(id);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void Borrow_WithAlreadyBorrowedBook_ReturnsBadRequest()
        {
            // Arrange
            var id = Guid.NewGuid();
            var borrowedBook = new Book { Id = id, IsBorrowed = true };
            _mockRepository.Setup(x => x.GetById(id)).Returns(borrowedBook);

            // Act
            var result = _controller.Borrow(id);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Book is already borrowed.", badRequestResult.Value);
        }

        [Fact]
        public void Return_WithValidId_ReturnsNoContent()
        {
            // Arrange
            var id = Guid.NewGuid();
            var book = new Book { Id = id, IsBorrowed = true };
            _mockRepository.Setup(x => x.GetById(id)).Returns(book);
            _mockRepository.Setup(x => x.Update(It.IsAny<Book>()));

            // Act
            var result = _controller.Return(id);

            // Assert
            Assert.IsType<NoContentResult>(result);
            Assert.False(book.IsBorrowed);
        }

        [Fact]
        public void Return_WithNonExistentId_ReturnsNotFound()
        {
            // Arrange
            var id = Guid.NewGuid();
            _mockRepository.Setup(x => x.GetById(id)).Returns((Book?)null);

            // Act
            var result = _controller.Return(id);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void Return_WithNotBorrowedBook_ReturnsBadRequest()
        {
            // Arrange
            var id = Guid.NewGuid();
            var notBorrowedBook = new Book { Id = id, IsBorrowed = false };
            _mockRepository.Setup(x => x.GetById(id)).Returns(notBorrowedBook);

            // Act
            var result = _controller.Return(id);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Book is not borrowed.", badRequestResult.Value);
        }

    }
}
