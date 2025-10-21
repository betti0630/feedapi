using AttrectoTest.Persistence.Seed;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AttrectoTest.ApiService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SeedController : Controller
    {
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult> Get(IDbSeeder seeder, CancellationToken cancellationToken)
        {
            await seeder.SeedAsync();
            return Ok();
        }

    }
}
