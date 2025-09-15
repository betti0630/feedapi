using AttrectoTest.Application.Features.Feed.Dtos;
using AttrectoTest.Domain;


namespace AttrectoTest.Application.Features.Feed.Mappers;

public class FeedMapper
{
    public FeedDto MapFeedToDto(Domain.Feed feed, int likeCount, int userId)
    {
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
        dto.AuthorUserName = feed.Author.UserName;
        dto.PublishedAt = feed.PublishedAt;
        dto.IsOwnFeed = feed.AuthorId == userId;
        dto.LikeCount = likeCount;
        
        if (feed is ImageFeed imageFeed && dto is ImageFeedDto imageDto)
        {
            imageDto.ImageData = imageFeed.ImageData;
        }
        if (feed is VideoFeed videoFeed && dto is VideoFeedDto videoDto)
        {
            videoDto.VideoUrl = videoFeed.VideoUrl;
        }

        return dto;
    }
}
