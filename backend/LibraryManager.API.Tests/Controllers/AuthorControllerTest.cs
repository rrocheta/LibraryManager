using AutoFixture;
using LibraryManager.API.Controllers;
using LibraryManager.API.Dtos;
using LibraryManager.API.Models;
using LibraryManager.API.Repositories;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace LibraryManager.API.Tests.Controllers
{
    public class AuthorControllerTest
    {
        private readonly Mock<IAuthorRepository> _mockRepository;
        private readonly AuthorsController _controller;
        private readonly Fixture _fixture;

        public AuthorControllerTest()
        {
            _mockRepository = new Mock<IAuthorRepository>();
            _controller = new AuthorsController(_mockRepository.Object);
            _fixture = new Fixture();

            // Customize to avoid recursion (since Author may reference Books and Book may reference Authors, etc)
            _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
                .ForEach(b => _fixture.Behaviors.Remove(b));
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
        }

        [Fact]
        public void GetAll_ShouldReturnAllAuthors()
        {
            // Arrange
            var authors = _fixture.CreateMany<Author>(3).ToList();
            _mockRepository.Setup(r => r.GetAll(It.IsAny<string>())).Returns(authors);

            // Act
            var result = _controller.GetAll();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnAuthors = Assert.IsAssignableFrom<IEnumerable<AuthorDto>>(okResult.Value);
            Assert.Equal(3, returnAuthors.Count());
        }

        [Fact]
        public void GetAll_WhenNoAuthors_ReturnsNotFound()
        {
            // Arrange
            _mockRepository.Setup(r => r.GetAll(null)).Returns(Enumerable.Empty<Author>());

            // Act
            var result = _controller.GetAll();

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            Assert.Equal("No authors found.", notFoundResult.Value);
        }

        [Fact]
        public void GetAll_WhenAuthorsIsNull_ReturnsNotFound()
        {
            // Arrange
            _mockRepository.Setup(r => r.GetAll(null)).Returns((IEnumerable<Author>)null);

            // Act
            var result = _controller.GetAll();

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            Assert.Equal("No authors found.", notFoundResult.Value);
        }
    }
}
