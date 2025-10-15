using AttrectoTest.Web.Services.Base;
using AttrectoTest.Web.Shared.Models;

using AutoMapper;

namespace AttrectoTest.Web.Services;

public class MapperProfile : Profile
{
    public MapperProfile() { 
        CreateMap<FeedDto, FeedItemModel>().ReverseMap();
        CreateMap<ImageFeedDto, ImageFeedItemModel>().IncludeBase<FeedDto, FeedItemModel>().ReverseMap();
        CreateMap<VideoFeedDto, VideoFeedItemModel>().IncludeBase<ImageFeedDto, ImageFeedItemModel>().ReverseMap();
        CreateMap<RssFeedDto, RssFeedItemModel>().IncludeBase<FeedDto, FeedItemModel>().ReverseMap();
    }
}
