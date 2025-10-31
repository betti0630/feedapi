using FeedApp.Application.Features.Feeds.Dtos;
using FeedApp.Blazor.Common.Models;

using AutoMapper;

namespace FeedApp.BlazorWeb.Services;

internal class MapperProfile : Profile
{
    public MapperProfile() { 
        CreateMap<FeedDto, FeedItemModel>().ReverseMap();
        CreateMap<ImageFeedDto, ImageFeedItemModel>().IncludeBase<FeedDto, FeedItemModel>().ReverseMap();
        CreateMap<VideoFeedDto, VideoFeedItemModel>().IncludeBase<ImageFeedDto, ImageFeedItemModel>().ReverseMap();
        CreateMap<RssFeedDto, RssFeedItemModel>().IncludeBase<FeedDto, FeedItemModel>().ReverseMap();
    }
}
