using AttrectoTest.Application.Contracts.Identity;
using AttrectoTest.Application.Features.Feed.Dtos;
using AttrectoTest.Domain;

using MediatR;


namespace AttrectoTest.Application.Features.Feed.Mappers;

public static class FeedMapper
{
    public static async Task<FeedDto> MapFeedToDto(this Domain.Feed feed, int likeCount, bool isLiked, int userId, IIamService iamService)
    {
        ArgumentNullException.ThrowIfNull(iamService);
        ArgumentNullException.ThrowIfNull(feed);
        FeedDto dto = feed switch
        {
            VideoFeed => new VideoFeedDto(),
            ImageFeed => new ImageFeedDto(),
            Domain.Feed => new FeedDto(),
            _ => throw new ArgumentException("Unknow type of feed", nameof(feed)),
        };
        
        dto.Id = feed.Id;
        dto.Title = feed.Title;
        dto.Content = feed.Content;
        dto.AuthorId = feed.AuthorId;
        dto.AuthorUserName = await iamService.GetUserNameByUserId(feed.AuthorId);
        dto.PublishedAt = feed.PublishedAt;
        dto.IsOwnFeed = feed.AuthorId == userId;
        dto.LikeCount = likeCount;
        dto.IsLiked = isLiked;
        
        if (feed is ImageFeed imageFeed && dto is ImageFeedDto imageDto)
        {
            imageDto.ImageUrl = $"{imageFeed.ImageUrl}";
        }
        if (feed is VideoFeed videoFeed && dto is VideoFeedDto videoDto)
        {
            videoDto.VideoUrl = videoFeed.VideoUrl;
        }

        return dto;
    }
}
