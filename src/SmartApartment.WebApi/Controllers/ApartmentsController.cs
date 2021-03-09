namespace SmartApartment.WebApi.Controllers
{
    using System.Threading;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;

    using SmartApartment.WebApi.Models;

    [ApiController]
    [Route("/")]
    [Produces(Constants.ContentTypes.ApplicationJson)]
    public class ApartmentsController : ControllerBase
    {

        private readonly ILogger<ApartmentsController> _logger;

        public ApartmentsController(ILogger<ApartmentsController> logger)
        {
            _logger = logger;
        }

        [HttpGet(nameof(Search), Name = nameof(Search))]
        public async Task<IActionResult> Search([FromQuery]SearchOptions searchOptions, CancellationToken cancellationToken)
        {
            return Ok();
        }

    }
}
