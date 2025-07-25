using LibraryManager.API.Dtos;
using LibraryManager.API.Models;
using LibraryManager.API.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManager.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PublisherController : ControllerBase
    {
        private readonly IPublisherRepository _publisherRepository;

        public PublisherController(IPublisherRepository publisherRepository)
        {
            _publisherRepository = publisherRepository;
        }

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
