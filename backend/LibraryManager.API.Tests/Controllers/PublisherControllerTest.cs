using AutoFixture;
using LibraryManager.API.Controllers;
using LibraryManager.API.Dtos;
using LibraryManager.API.Models;
using LibraryManager.API.Repositories;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace LibraryManager.API.Tests.Controllers
{
    public class PublisherControllerTest
    {
        private readonly Mock<IPublisherRepository> _mockRepository;
        private readonly PublisherController _controller;
        private readonly Fixture _fixture;


        public PublisherControllerTest()
        {
            _mockRepository = new Mock<IPublisherRepository>();
            _controller = new PublisherController(_mockRepository.Object);
            _fixture = new Fixture();

            // Customize to avoid recursion (since Publisher references Books and Book can reference Publisher, etc)
            _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
                .ForEach(b => _fixture.Behaviors.Remove(b));
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
        }

        [Fact]
        public void GetAll_ShouldReturnAllPublishers()
        {
            // Arrange
            var publishers = _fixture.CreateMany<Publisher>(3).ToList();
            _mockRepository.Setup(r => r.GetAll(It.IsAny<string>())).Returns(publishers);

            // Act
            var result = _controller.GetAll();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnPublishers = Assert.IsAssignableFrom<IEnumerable<PublisherDto>>(okResult.Value);
            Assert.Equal(3, returnPublishers.Count());
        }

        [Fact]
        public void GetAll_WhenNoPublishers_ReturnsNotFound()
        {
            // Arrange
            _mockRepository.Setup(r => r.GetAll(null)).Returns(Enumerable.Empty<Publisher>());
            // Act
            var result = _controller.GetAll();
            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            Assert.Equal("No publishers found.", notFoundResult.Value);
        }

        [Fact]
        public void GetAll_WhenPublishersIsNull_ReturnsNotFound()
        {
            // Arrange
            _mockRepository.Setup(r => r.GetAll(null)).Returns((IEnumerable<Publisher>)null);
            // Act
            var result = _controller.GetAll();
            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            Assert.Equal("No publishers found.", notFoundResult.Value);

        }
    }
}
