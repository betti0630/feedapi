using AttrectoTest.BlazorWasm.Services.Base;

using AutoMapper;

using FeedApp.Blazor.Common.Models;

namespace FeedApp.BlazorWasm.Services;

internal class MapperProfile : Profile
{
    public MapperProfile() { 
        CreateMap<FeedDto, FeedItemModel>().ReverseMap();
        CreateMap<ImageFeedDto, ImageFeedItemModel>().IncludeBase<FeedDto, FeedItemModel>().ReverseMap();
        CreateMap<VideoFeedDto, VideoFeedItemModel>().IncludeBase<ImageFeedDto, ImageFeedItemModel>().ReverseMap();
        CreateMap<RssFeedDto, RssFeedItemModel>().IncludeBase<FeedDto, FeedItemModel>().ReverseMap();
    }
}
