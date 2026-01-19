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

        /// <summary>
        /// Retrieves a single publisher by ID.
        /// </summary>
        /// <param name="id">Publisher identifier.</param>
        /// <returns>The publisher if found; otherwise 404.</returns>
        [HttpGet("{id}")]
        public ActionResult<PublisherDto> GetById(int id)
        {
            var publisher = _publisherRepository.GetById(id);
            if (publisher == null)
            {
                return NotFound();
            }

            return Ok(new PublisherDto { Id = publisher.Id, Name = publisher.Name });
        }

        /// <summary>
        /// Creates a new publisher.
        /// </summary>
        /// <param name="publisherDto">The DTO containing publisher data.</param>
        /// <returns>The created publisher.</returns>
        [HttpPost]
        public ActionResult<PublisherDto> Create([FromBody] CreatePublisherDto publisherDto)
        {
            if (publisherDto == null || string.IsNullOrWhiteSpace(publisherDto.Name))
            {
                return BadRequest("Invalid publisher data.");
            }

            var createdPublisher = _publisherRepository.Add(new Publisher
            {
                Name = publisherDto.Name.Trim()
            });

            var result = new PublisherDto { Id = createdPublisher.Id, Name = createdPublisher.Name };
            return CreatedAtAction(nameof(GetById), new { id = createdPublisher.Id }, result);
        }
    }
}
