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
        private readonly PublishersController _controller;
        private readonly Fixture _fixture;


        public PublisherControllerTest()
        {
            _mockRepository = new Mock<IPublisherRepository>();
            _controller = new PublishersController(_mockRepository.Object);
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

        [Fact]
        public void Delete_WhenPublisherMissing_ReturnsNotFound()
        {
            // Arrange
            var publisherId = 10;
            _mockRepository.Setup(r => r.GetById(publisherId)).Returns((Publisher)null);

            // Act
            var result = _controller.Delete(publisherId);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void Delete_WhenPublisherHasBooks_ReturnsBadRequest()
        {
            // Arrange
            var publisherId = 12;
            _mockRepository.Setup(r => r.GetById(publisherId)).Returns(new Publisher { Id = publisherId, Name = "Test" });
            _mockRepository.Setup(r => r.HasBooks(publisherId)).Returns(true);

            // Act
            var result = _controller.Delete(publisherId);

            // Assert
            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Cannot delete a publisher with associated books.", badRequest.Value);
        }

        [Fact]
        public void Delete_WhenPublisherHasNoBooks_ReturnsNoContent()
        {
            // Arrange
            var publisherId = 14;
            _mockRepository.Setup(r => r.GetById(publisherId)).Returns(new Publisher { Id = publisherId, Name = "Test" });
            _mockRepository.Setup(r => r.HasBooks(publisherId)).Returns(false);

            // Act
            var result = _controller.Delete(publisherId);

            // Assert
            Assert.IsType<NoContentResult>(result);
            _mockRepository.Verify(r => r.Remove(publisherId), Times.Once);
        }
    }
}
