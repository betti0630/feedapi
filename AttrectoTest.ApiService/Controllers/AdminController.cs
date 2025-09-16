using AttrectoTest.Application.Features.Feed.Commands.PurgeFeed;

using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AttrectoTest.ApiService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class AdminController(IMediator mediator) : ControllerBase
    {
        /// <summary>
        /// Purge soft deleted feedek 
        /// </summary>
        /// <returns>Job started</returns>
        [HttpPost, Route("maintenance/purgeFeeds")]
        public async Task<IActionResult> PurgeFeeds(CancellationToken cancellationToken)
        {
            var command = new PurgeFeedCommand();
            await mediator.Send(command, cancellationToken);
            return NoContent();
        }

    }
}
