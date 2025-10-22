﻿using AttrectoTest.Iam.Persistence.Seed;

using Grpc.Core;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AttrectoTest.IamService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
#pragma warning disable CA1515 // Consider making public types internal
    public class SeedController : Controller
#pragma warning restore CA1515 // Consider making public types internal
    {
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult> Get(IDbSeeder seeder, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(seeder);
            await seeder.SeedAsync();
            return Ok();
        }

    }
}
