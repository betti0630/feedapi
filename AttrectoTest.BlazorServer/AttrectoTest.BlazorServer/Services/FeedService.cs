using AttrectoTest.Application.Features.Feed.Queries.ListFeeds;
using AttrectoTest.Application.Models;
using AttrectoTest.Blazor.Shared.Contracts;
using AttrectoTest.Blazor.Shared.Models;
using AttrectoTest.Domain;

using AutoMapper;

using MediatR;

using Microsoft.AspNetCore.Components;

using System.Runtime;


namespace AttrecotTest.BlazorServer.Services;

public class FeedService : IFeedService
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

    public Task<bool> AddLike(int feedId)
    {
        throw new NotImplementedException();
    }

    public Task DeleteFeed(int feedId)
    {
        throw new NotImplementedException();
    }

    public Task<bool> DeleteLike(int feedId)
    {
        throw new NotImplementedException();
    }

    public async Task<FeedListModel> GetFeeds()
    {
        var query = new ListFeedsQuery
        {
            IncludeExternal =  false,
            Page = 1,
            PageSize = 100,
            Sort = ListSort.CreatedAt_desc
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
