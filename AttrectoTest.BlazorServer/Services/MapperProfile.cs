using AttrectoTest.Application.Features.Feed.Dtos;
using AttrectoTest.Blazor.Common.Models;
using AutoMapper;

namespace AttrectoTest.BlazorServer.Services;

internal class MapperProfile : Profile
{
    public MapperProfile() { 
        CreateMap<FeedDto, FeedItemModel>().ReverseMap();
        CreateMap<ImageFeedDto, ImageFeedItemModel>().IncludeBase<FeedDto, FeedItemModel>().ReverseMap();
        CreateMap<VideoFeedDto, VideoFeedItemModel>().IncludeBase<ImageFeedDto, ImageFeedItemModel>().ReverseMap();
        CreateMap<RssFeedDto, RssFeedItemModel>().IncludeBase<FeedDto, FeedItemModel>().ReverseMap();
    }
}
