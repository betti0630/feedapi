using AttrectoTest.Application.Features.Feed.Commands.AddLike;
using AttrectoTest.Application.Features.Feed.Commands.CreateFeed;
using AttrectoTest.Application.Features.Feed.Commands.DeleteFeed;
using AttrectoTest.Application.Features.Feed.Commands.DeleteLike;
using AttrectoTest.Application.Features.Feed.Queries.ListFeeds;
using AttrectoTest.Application.Models;
using AttrectoTest.Blazor.Common.Contracts;
using AttrectoTest.Blazor.Common.Models;
using AttrectoTest.Domain;

using AutoMapper;

using MediatR;

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

using System.Runtime;
using System.Threading;


namespace AttrectoTest.BlazorWeb.Services;

internal class FeedService : IFeedService
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;
    private readonly NavigationManager _navigationManager;

    public FeedService(IMediator mediator, IMapper mapper, NavigationManager navigationManager)
    {
        _mediator = mediator;
        _mapper = mapper;
        _navigationManager = navigationManager;
    }

    public async Task AddFeed(FeedEditModel feed)
    {
        CreateFeedCommand command;
        if (!string.IsNullOrEmpty(feed.VideoUrl))
        {
            command = new CreateVideoFeedCommand
            {
                VideoUrl = feed.VideoUrl,
                ImageUrl = feed.ImageUrl
            };
        } else if (!string.IsNullOrEmpty(feed.ImageUrl))
        {
            command = new CreateImageFeedCommand
            {
                ImageUrl = feed.ImageUrl,
            };
        }
        else
        {
            command = new CreateFeedCommand();
        }

        command.Title = feed.Title;
        command.Content = feed.Content;
        await _mediator.Send(command);
    }

    public async Task<bool> AddLike(int feedId)
    {
        var command = new AddLikeCommand { FeedId = feedId };
        await _mediator.Send(command);
        return true;
    }

    public async Task DeleteFeed(int feedId)
    {
        var command = new DeleteFeedCommand { Id = feedId };
        await _mediator.Send(command);
    }

    public async Task<bool> DeleteLike(int feedId)
    {
        var command = new DeleteLikeCommand { FeedId = feedId };
        await _mediator.Send(command);
        return true;
    }

    public async Task<FeedListModel> GetFeeds()
    {
        var query = new ListFeedsQuery
        {
            IncludeExternal =  true,
            Page = 1,
            PageSize = 100,
            Sort = ListSort.CreatedAtDesc
        };
        var result = await _mediator.Send(query);
        var model = new FeedListModel();
        model.Items = _mapper.Map<List<FeedItemModel>>(result.Items.ToList());
        var baseUri = new Uri(_navigationManager.BaseUri);
        foreach (var feed in model.Items)
        {
            if (feed is ImageFeedItemModel imageFeed)
            {
                imageFeed.ImageUrl = new Uri(baseUri, imageFeed.ImageUrl).ToString();
            }
        }
        return model;
    }
}
