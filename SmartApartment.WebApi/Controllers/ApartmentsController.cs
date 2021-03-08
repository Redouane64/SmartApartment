namespace SmartApartment.WebApi.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    [ApiController]
    [Route("/")]
    public class ApartmentsController : ControllerBase
    {

        private readonly ILogger<ApartmentsController> _logger;

        public ApartmentsController(ILogger<ApartmentsController> logger)
        {
            _logger = logger;
        }

        [HttpGet(nameof(Search), Name = nameof(Search))]
        public async Task<IActionResult> Search()
        {
            return Ok();
        }

    }
}
