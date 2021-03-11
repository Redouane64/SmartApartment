namespace SmartApartment.WebApi.Controllers
{
    using System.Threading;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using SmartApartment.Common.Abstraction;
    using SmartApartment.WebApi.Models;

    [ApiController]
    [Route("/")]
    [Produces(Constants.ContentTypes.ApplicationJson)]
    public class ApartmentsController : ControllerBase
    {
        private readonly ISearchService searchService;
        private readonly ILogger<ApartmentsController> _logger;

        public ApartmentsController(ISearchService searchService, ILogger<ApartmentsController> logger)
        {
            this.searchService = searchService;
            this._logger = logger;
        }

        [HttpGet(nameof(Search), Name = nameof(Search))]
        public async Task<IActionResult> Search([FromQuery] SearchOptions searchOptions, CancellationToken cancellationToken)
        {
            return Ok(await this.searchService.Search(null));
        }

    }
}
