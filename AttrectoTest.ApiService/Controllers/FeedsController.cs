using AttrectoTest.ApiService.Validators;
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


namespace AttrectoTest.ApiService.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class FeedsController(IMediator mediator) : ControllerBase
{

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
            Sort = sort ?? ListSort.CreatedAtDesc
        };
        var result = await mediator.Send(query, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Get feed
    /// </summary>
    /// <returns>OK</returns>
    [HttpGet("{feedId}")]
    public async Task<ActionResult<FeedDto>> Get(int feedId, CancellationToken cancellationToken)
    {
        var query = new GetFeedQuery { Id = feedId };
        var response = await mediator.Send(query, cancellationToken);
        return Ok(response);
    }


    /// <summary>
    /// Create new feed
    /// </summary>
    /// <returns>Created</returns>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(CreateFeedCommandResponse))]
    [ProducesResponseType(400)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<CreateFeedCommandResponse>> Post([FromBody] CreateFeedCommand feed, CancellationToken cancellationToken)
    {
        var response = await mediator.Send(feed, cancellationToken);
        return CreatedAtAction(nameof(Post), response);
    }


    /// <summary>
    /// Create new image feed
    /// </summary>
    /// <returns>Created</returns>
    [HttpPost("image")]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(CreateFeedCommandResponse))]
    [Consumes("multipart/form-data")]
    public async Task<ActionResult<CreateFeedCommandResponse>> CreateImageFeed([FromServices] IImageFileProcessor imageFileProcessor, [FromForm] ImageFeedCreateDto feed, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(imageFileProcessor);
        ArgumentNullException.ThrowIfNull(feed);

        var imageUrl = await imageFileProcessor.ValidateAndGetUrlOfImage(feed.File, cancellationToken);
        
        var command = new CreateImageFeedCommand
        {
            Title = feed.Title,
            Content = feed.Content,
            ImageUrl = imageUrl
        };
        var response = await mediator.Send(command, cancellationToken);
        return CreatedAtAction(nameof(Post), response);
     }

    /// <summary>
    /// Create new video feed
    /// </summary>
    /// <returns>Created</returns>
    [HttpPost("video")]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(CreateFeedCommandResponse))]
    [Consumes("multipart/form-data")]
    public async Task<ActionResult<CreateFeedCommandResponse>> CreateVideoFeed([FromServices] IImageFileProcessor imageFileProcessor, [FromForm] VideoFeedCreateDto feed, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(imageFileProcessor);
        ArgumentNullException.ThrowIfNull(feed);

        var imageUrl = await imageFileProcessor.ValidateAndGetUrlOfImage(feed.File, cancellationToken);

        var command = new CreateVideoFeedCommand
        {
            Title = feed.Title,
            Content = feed.Content,
            ImageUrl = imageUrl,
            VideoUrl = feed.VideoUrl
        };
        var response = await mediator.Send(command, cancellationToken);
        return CreatedAtAction(nameof(Post), response);
    }

    /// <summary>
    /// Update feed
    /// </summary>
    /// <returns>OK</returns>
    [HttpPatch("{feedId}")]
    public async Task<ActionResult<FeedDto>> Patch(int feedId, [FromBody] UpdateFeedCommand feed, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(feed);

        if (feedId != feed.Id)
        {
            throw new ArgumentException("Feed ID mismatch");
        }
        var response = await mediator.Send(feed, cancellationToken);
        return Ok(response);
    }

    /// <summary>
    /// Update image feed
    /// </summary>
    /// <returns>OK</returns>
    [HttpPatch("image/{feedId}")]
    [Consumes("multipart/form-data")]
    public async Task<ActionResult<FeedDto>> PatchImageFeed([FromServices] IImageFileProcessor imageFileProcessor, int feedId, [FromForm] ImageFeedUpdateDto feed, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(imageFileProcessor);
        ArgumentNullException.ThrowIfNull(feed);

        if (feedId != feed.Id)
        {
            throw new ArgumentException("Feed ID mismatch");
        }
        var command = new UpdateImageFeedCommand
        {
            Id = feed.Id,
            Title = feed.Title,
            Content = feed.Content
        };
        if (feed.File is not null) {
            var imageUrl = await imageFileProcessor.ValidateAndGetUrlOfImage(feed.File, cancellationToken);
            command.ImageUrl = imageUrl;
        }
        var response = await mediator.Send(command, cancellationToken);
        return Ok(response);
    }

    /// <summary>
    /// Update video feed
    /// </summary>
    /// <returns>OK</returns>
    [HttpPatch("video/{feedId}")]
    [Consumes("multipart/form-data")]
    public async Task<ActionResult<FeedDto>> PatchVideoFeed([FromServices] IImageFileProcessor imageFileProcessor, int feedId, [FromForm] VideoFeedUpdateDto feed, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(imageFileProcessor);
        ArgumentNullException.ThrowIfNull(feed);

        if (feedId != feed.Id)
        {
            throw new ArgumentException("Feed ID mismatch");
        }
        var command = new UpdateVideoFeedCommand
        {
            Id = feed.Id,
            Title = feed.Title,
            Content = feed.Content,
            VideoUrl = feed.VideoUrl
        };
        if (feed.File is not null)
        {
            var imageUrl = await imageFileProcessor.ValidateAndGetUrlOfImage(feed.File, cancellationToken);
            command.ImageUrl = imageUrl;
        }
        var response = await mediator.Send(command, cancellationToken);
        return Ok(response);
    }

    /// <summary>
    /// Delete feed 
    /// </summary>
    /// <returns>Soft deleted</returns>
    [HttpDelete("{feedId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(400)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesDefaultResponseType]
    public async Task<IActionResult> Delete(int feedId, CancellationToken cancellationToken)
    {
        var command = new DeleteFeedCommand { Id = feedId };
        await mediator.Send(command, cancellationToken);
        return NoContent();
    }


    #region Like

    /// <summary>
    /// Feed like
    /// </summary>
    /// <returns>OK</returns>
    [HttpPost, Route("{feedId}/like", Name = "likePOST")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(400)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesDefaultResponseType]
    public async Task<IActionResult> LikePOST(int feedId, CancellationToken cancellationToken)
    {
        var command = new AddLikeCommand { FeedId = feedId };
        await mediator.Send(command, cancellationToken);
        return NoContent();
    }

    /// <summary>
    /// Like delete
    /// </summary>
    /// <returns>OK</returns>
    [HttpDelete, Route("{feedId}/like", Name = "likeDELETE")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(400)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesDefaultResponseType]
    public async Task<IActionResult> LikeDELETE(int feedId, CancellationToken cancellationToken)
    {

        var command = new DeleteLikeCommand { FeedId = feedId };
        await mediator.Send(command, cancellationToken);
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
        var result = await mediator.Send(query, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Create new comment
    /// </summary>
    /// <returns>Created</returns>
    [HttpPost, Route("{feedId}/comments", Name = "commentsPOST")]
    public async Task<ActionResult<CommentDto>> CommentsPOST([FromBody] CreateCommentCommand request, int feedId, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        if (feedId != request.FeedId)
        {
            throw new ArgumentException("Feed ID mismatch");
        }
        var result = await mediator.Send(request, cancellationToken);
        return CreatedAtAction(nameof(CommentsPOST), result);
    }

    /// <summary>
    /// Update comment 
    /// </summary>
    /// <returns>OK</returns>
    [HttpPatch, Route("{feedId}/comments/{commentId}", Name = "commentsPATCH")]
    public async Task<ActionResult<CommentDto>> CommentsPATCH([FromBody] UpdateCommentCommand request, int feedId, int commentId, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        if (feedId != request.FeedId)
        {
            throw new ArgumentException("Feed ID mismatch");
        }
        if (commentId != request.CommentId)
        {
            throw new ArgumentException("Comment ID mismatch");
        }
        var result = await mediator.Send(request, cancellationToken);
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
        await mediator.Send(command, cancellationToken);
        return NoContent();
    }


    #endregion
}
