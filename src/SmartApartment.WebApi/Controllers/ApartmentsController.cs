namespace SmartApartment.WebApi.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using AutoMapper;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using SmartApartment.Common.Abstraction;
    using SmartApartment.WebApi.Models;
    using SmartApartment.WebApi.ViewModels;

    [ApiController]
    [Route("/")]
    [Produces(Constants.ContentTypes.ApplicationJson)]
    public class ApartmentsController : ControllerBase
    {
        private readonly ISearchService searchService;
        private readonly IMapper mapper;
        private readonly ILogger<ApartmentsController> _logger;

        public ApartmentsController(ISearchService searchService,
                                    IMapper mapper,
                                    ILogger<ApartmentsController> logger)
        {
            this.searchService = searchService;
            this.mapper = mapper;
            this._logger = logger;
        }

        [HttpGet(nameof(Search), Name = nameof(Search))]
        [ProducesResponseType(typeof(CollectionResult<DocumentViewModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Search([FromQuery] SearchOptions searchOptions,
                                                CancellationToken cancellationToken)
        {
            var searchResult = await this.searchService.Search(
                searchOptions.Keyword, 
                searchOptions.Market, 
                searchOptions.Limit,
                cancellationToken
            );

            var documents = this.mapper.Map<IEnumerable<DocumentViewModel>>(searchResult);

            return Ok(new CollectionResult<DocumentViewModel>(
                searchResult.Count(),
                documents
            ));
        }

    }
}
