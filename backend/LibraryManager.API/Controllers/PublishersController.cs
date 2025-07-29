using LibraryManager.API.Dtos;
using LibraryManager.API.Models;
using LibraryManager.API.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManager.API.Controllers
{
    /// <summary>
    /// Controller for managing publisher-related API endpoints.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class PublishersController : ControllerBase
    {
        private readonly IPublisherRepository _publisherRepository;

        public PublishersController(IPublisherRepository publisherRepository)
        {
            _publisherRepository = publisherRepository;
        }

        /// <summary>
        /// Retrieves all publishers.
        /// </summary>
        /// <returns>
        /// Returns a list of publishers as <see cref="PublisherDto"/>.  
        /// Returns 404 if no publishers are found.
        /// </returns>
        [HttpGet]
        public ActionResult<IEnumerable<Publisher>> GetAll()
        {
            var publishers = _publisherRepository.GetAll();
            if (publishers == null || !publishers.Any())
            {
                return NotFound("No publishers found.");
            }

            var result = publishers.Select(pub => new PublisherDto { Id = pub.Id, Name = pub.Name });
            return Ok(result);
        }
    }
}
