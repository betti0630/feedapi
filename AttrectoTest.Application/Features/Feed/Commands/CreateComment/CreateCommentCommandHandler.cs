﻿
using AttrectoTest.Application.Contracts.Persistence;
using AttrectoTest.Application.Features.Feed.Dtos;

using MediatR;

namespace AttrectoTest.Application.Features.Feed.Commands.CreateComment;

#pragma warning disable CA1812
internal sealed class CreateCommentCommandHandler(IFeedRepository feedRepository, ICommentRepository commentRepository) : IRequestHandler<CreateCommentCommand, CommentDto>
{
    public async Task<CommentDto> Handle(CreateCommentCommand request, CancellationToken cancellationToken)
    {
        var feed = await feedRepository.GetByIdAsync(request.FeedId, cancellationToken).ConfigureAwait(false) ?? throw new KeyNotFoundException($"Feed with id {request.FeedId} not found");
        if (feed.IsDeleted)
        {
            throw new KeyNotFoundException($"Feed with id {request.FeedId} not found");
        }
        var comment = new Domain.Comment
        {
            Content = request.Content,
            UserId = request.UserId,
            FeedId = request.FeedId
        };
        await commentRepository.CreateAsync(comment, cancellationToken).ConfigureAwait(false);
        return new CommentDto(feed.Id, comment.Id, comment.Content, comment.DateCreated, comment.DateModified, comment.UserId, request.UserId);
    } 
 }
