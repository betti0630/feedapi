using AttrectoTest.Application.Features.Feed.Commands.AddLike;
using AttrectoTest.Application.Features.Feed.Commands.CreateComment;
using AttrectoTest.Application.Features.Feed.Commands.CreateFeed;
using AttrectoTest.Application.Features.Feed.Commands.DeleteComment;
using AttrectoTest.Application.Features.Feed.Commands.DeleteFeed;
using AttrectoTest.Application.Features.Feed.Commands.DeleteLike;
using AttrectoTest.Application.Features.Feed.Commands.UpdateComment;
using AttrectoTest.Application.Features.Feed.Commands.UpdateFeed;
using AttrectoTest.Application.Features.Feed.Dtos;
using AttrectoTest.Application.Features.Feed.Queries.GetFeed;
using AttrectoTest.Application.Features.Feed.Queries.ListComments;
using AttrectoTest.Application.Features.Feed.Queries.ListFeeds;
using AttrectoTest.Application.Models;

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

        /// <summary>
        /// List feeds
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <param name="includeExternal">Include external users' feeds</param>"
        /// <returns>OK</returns>
        [HttpGet]
        public async Task<ActionResult<PagedFeeds>> Get( [FromQuery] int? page, [FromQuery] int? pageSize, [FromQuery] ListSort? sort, [FromQuery] bool? includeExternal, CancellationToken cancellationToken)
        {
            var query = new ListFeedsQuery
            {
                IncludeExternal = includeExternal ?? false,
                Page = page,
                PageSize = pageSize,
                Sort = sort ?? ListSort.CreatedAt_desc
            };
            var result = await _mediator.Send(query, cancellationToken);
            return Ok(result);
        }

        /// <summary>
        /// Get feed
        /// </summary>
        /// <returns>OK</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<FeedDto>> Get(int id, CancellationToken cancellationToken)
        {
            var query = new GetFeedQuery { Id = id };
            var response = await _mediator.Send(query, cancellationToken);
            return Ok(response);
        }


        /// <summary>
        /// Create new feed
        /// </summary>
        /// <returns>Created</returns>
        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<FeedDto>> Post([FromBody] CreateFeedCommand feed, CancellationToken cancellationToken)
        {
            var response = await _mediator.Send(feed, cancellationToken);
            return CreatedAtAction(nameof(Post), new { id = response });
        }


        /// <summary>
        /// Update feed
        /// </summary>
        /// <returns>OK</returns>
        [HttpPatch("{id}")]
        public async Task<ActionResult<Feed>> Put(int id, [FromBody] UpdateFeedCommand feed, CancellationToken cancellationToken)
        {
            if (id != feed.Id)
            {
                throw new ArgumentException("Feed ID mismatch");
            }
            var response = await _mediator.Send(feed, cancellationToken);
            return Ok(response);
        }


        /// <summary>
        /// Delete feed 
        /// </summary>
        /// <returns>Soft deleted</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
        {
            var command = new DeleteFeedCommand { Id = id };
            await _mediator.Send(command, cancellationToken);
            return NoContent();
        }


        #region Like

        /// <summary>
        /// Feed like
        /// </summary>
        /// <returns>OK</returns>
        [HttpPost, Route("{feedId}/like", Name = "likePOST")]
        public async Task<IActionResult> LikePOST(int feedId, CancellationToken cancellationToken)
        {
            var command = new AddLikeCommand { FeedId = feedId };
            await _mediator.Send(command, cancellationToken);
            return NoContent();
        }

        /// <summary>
        /// Like delete
        /// </summary>
        /// <returns>OK</returns>
        [HttpDelete, Route("{feedId}/like", Name = "likeDELETE")]
        public async Task<IActionResult> LikeDELETE(int feedId, CancellationToken cancellationToken)
        {

            var command = new DeleteLikeCommand { FeedId = feedId };
            await _mediator.Send(command, cancellationToken);
            return NoContent();
        }

        #endregion

        #region Comments

        /// <summary>
        /// List comments
        /// </summary>
        /// <returns>OK</returns>
        [HttpGet, Route("{feedId}/comments", Name = "commentsGET")]
        public async Task<ActionResult<PagedComments>> CommentsGET([FromQuery] int? page, [FromQuery] int? pageSize, int feedId, CancellationToken cancellationToken)
        {
            var query = new ListCommentsQuery
            {
                FeedId = feedId,
                Page = page ?? 1,
                PageSize = pageSize ?? 20
            };
            var result = await _mediator.Send(query, cancellationToken);
            return Ok(result);
        }

        /// <summary>
        /// Create new comment
        /// </summary>
        /// <returns>Created</returns>
        [HttpPost, Route("{feedId}/comments", Name = "commentsPOST")]
        public async Task<ActionResult<CommentDto>> CommentsPOST([FromBody] CreateCommentCommand request, int feedId, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(request, cancellationToken);
            return CreatedAtAction(nameof(CommentsPOST), new { id = result.Id });
        }

        /// <summary>
        /// Update comment 
        /// </summary>
        /// <returns>OK</returns>
        [HttpPatch, Route("{feedId}/comments/{commentId}", Name = "commentsPATCH")]
        public async Task<ActionResult<CommentDto>> CommentsPATCH([FromBody] UpdateCommentCommand request, int feedId, int commentId, CancellationToken cancellationToken)
        {
            if (commentId != request.CommentId)
            {
                throw new ArgumentException("Comment ID mismatch");
            }
            var result = await _mediator.Send(request, cancellationToken);
            return Ok(result);
        }

        /// <summary>
        /// Delete comment 
        /// </summary>
        /// <returns>Deleted</returns>
        [HttpDelete, Route("{feedId}/comments/{commentId}", Name = "commentsDELETE")]
        public async Task<IActionResult> CommentsDELETE(int feedId, int commentId, CancellationToken cancellationToken)
        {
            var command = new DeleteCommentCommand { FeedId = feedId, CommentId = commentId };
            await _mediator.Send(command, cancellationToken);
            return NoContent();
        }


        #endregion
    }
}
