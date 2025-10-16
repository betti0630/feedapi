using AttrectoTest.Blazor.Shared.Models;
using AttrectoTest.BlazorWasm.Services.Base;

using AutoMapper;

namespace AttrectoTest.BlazorWasm.Services;

public class MapperProfile : Profile
{
    public MapperProfile() { 
        CreateMap<FeedDto, FeedItemModel>().ReverseMap();
        CreateMap<ImageFeedDto, ImageFeedItemModel>().IncludeBase<FeedDto, FeedItemModel>().ReverseMap();
        CreateMap<VideoFeedDto, VideoFeedItemModel>().IncludeBase<ImageFeedDto, ImageFeedItemModel>().ReverseMap();
        CreateMap<RssFeedDto, RssFeedItemModel>().IncludeBase<FeedDto, FeedItemModel>().ReverseMap();
    }
}
