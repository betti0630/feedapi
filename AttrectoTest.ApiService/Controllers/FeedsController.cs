using AttrectoTest.Application.Features.Feed.Commands;

using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AttrectoTest.ApiService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class FeedsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public FeedsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // GET: api/<FeedsController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<FeedsController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<FeedsController>
        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> Post(CreateFeedCommand feed)
        {
            var response = await _mediator.Send(feed);
            return CreatedAtAction(nameof(Post), new { id = response });
        }

        // PUT api/<FeedsController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<FeedsController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
