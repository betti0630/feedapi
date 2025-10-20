using AttrectoTest.Aim.Application.Contracts.Identity;
using AttrectoTest.Aim.Persistence.Seed;

using Grpc.Core;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AttrectoTest.AimService.Controllers
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
