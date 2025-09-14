using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AttrectoTest.ApiService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        /// <summary>
        /// Purge soft deleted feedek 
        /// </summary>
        /// <returns>Job started</returns>
        [HttpPost, Route("maintenance/purge-soft-deleted", Name = "purge-soft-deleted")]
        public async Task<IActionResult> PurgeSoftDeleted(CancellationToken cancellationToken)
        {
            return NoContent();
        }

    }
}
