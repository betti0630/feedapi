using AttrectoTest.Application.Contracts.Logging;
using AttrectoTest.Application.Contracts.Persistence;
using AttrectoTest.Application.Exceptions;
using AttrectoTest.Application.Features.Feed.Commands.UpdateFeed;
using AttrectoTest.Domain;

using Moq;

namespace AttrectoTest.Application.Tests.Features.Feed.Commands
{
    public class UpdateFeedCommandHandlerTests
    {
        private readonly Mock<IFeedRepository> _feedRepositoryMock;
        private readonly Mock<IAppLogger<UpdateFeedCommandHandler>> _loggerMock;
        private readonly UpdateFeedCommandHandler _handler;

        public UpdateFeedCommandHandlerTests()
        {
            _feedRepositoryMock = new Mock<IFeedRepository>();
            _loggerMock = new Mock<IAppLogger<UpdateFeedCommandHandler>>();
            _handler = new UpdateFeedCommandHandler(_feedRepositoryMock.Object, _loggerMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldThrowBadRequest_WhenValidationFails()
        {
            // Arrange
            var longtitle = new string('A', 101); 
            var request = new UpdateFeedCommand { Id = 1, UserId = 1, Title = longtitle }; 

            // Act & Assert
            await Assert.ThrowsAsync<BadRequestException>(() => _handler.Handle(request, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_ShouldThrowNotFound_WhenFeedNotFound()
        {
            // Arrange
            var request = new UpdateFeedCommand { Id = 1, UserId = 1, Title = "Valid title" };
            _feedRepositoryMock.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>()))
                               .ReturnsAsync((Domain.Feed)null!);

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(request, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_ShouldThrowBadRequest_WhenFeedIsDeleted()
        {
            // Arrange
            var feed = new Domain.Feed { Id = 1, AuthorId = 1, IsDeleted = true };
            var request = new UpdateFeedCommand { Id = 1, UserId = 1, Title = "Valid title" };
            _feedRepositoryMock.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>()))
                               .ReturnsAsync(feed);

            // Act & Assert
            await Assert.ThrowsAsync<BadRequestException>(() => _handler.Handle(request, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_ShouldThrowBadRequest_WhenUserNotAuthor()
        {
            // Arrange
            var feed = new Domain.Feed { Id = 1, AuthorId = 2, Title = "Old", IsDeleted = false };
            var request = new UpdateFeedCommand { Id = 1, UserId = 1, Title = "New" };
            _feedRepositoryMock.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>()))
                               .ReturnsAsync(feed);

            // Act & Assert
            await Assert.ThrowsAsync<BadRequestException>(() => _handler.Handle(request, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_ShouldThrowBadRequest_WhenVideoFeedAndNoValidVideoUrl()
        {
            // Arrange
            var feed = new VideoFeed { Id = 1, AuthorId = 1, Title = "Old", IsDeleted = false };
            var request = new UpdateVideoFeedCommand { Id = 1, UserId = 1, Title = "New", VideoUrl = "invalid-url" };
            _feedRepositoryMock.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>()))
                               .ReturnsAsync(feed);
            // Act & Assert
            await Assert.ThrowsAsync<BadRequestException>(() => _handler.Handle(request, CancellationToken.None));
        }


        [Fact]
        public async Task Handle_ShouldUpdateFeedSuccessfully()
        {
            // Arrange
            var feed = new Domain.Feed { Id = 1, AuthorId = 1, Title = "Old", Content = "Old content", IsDeleted = false };
            var request = new UpdateFeedCommand { Id = 1, UserId = 1, Title = "New title", Content = "New content" };

            _feedRepositoryMock.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>()))
                               .ReturnsAsync(feed);

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.Equal(1, result.Id);
            Assert.Equal("New title", feed.Title);
            Assert.Equal("New content", feed.Content);
            _feedRepositoryMock.Verify(r => r.UpdateAsync(feed, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task HandleImageFeed_ShouldUpdateImageUrl()
        {
            // Arrange
            var feed = new ImageFeed { Id = 1, AuthorId = 1, Title = "Old", ImageUrl = "old.png" };
            var request = new UpdateImageFeedCommand { Id = 1, UserId = 1, Title = "New", ImageUrl = "new.png" };

            _feedRepositoryMock.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>()))
                               .ReturnsAsync(feed);

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.Equal("New", feed.Title);
            Assert.Equal("new.png", feed.ImageUrl);
            _feedRepositoryMock.Verify(r => r.UpdateAsync(feed, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task HandleVideoFeed_ShouldUpdateImageAndVideoUrl()
        {
            // Arrange
            var feed = new VideoFeed { Id = 1, AuthorId = 1, Title = "Old", Content= "Old", ImageUrl = "old.png", VideoUrl = "old.mp4" };
            var newUrl = "https://example.com/videos/family.mp4";
            var request = new UpdateVideoFeedCommand
            {
                Id = 1,
                UserId = 1,
                Title = "New",
                ImageUrl = "new.png",
                VideoUrl = newUrl
            };

            _feedRepositoryMock.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>()))
                               .ReturnsAsync(feed);

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.Equal("New", feed.Title);
            Assert.Equal("Old", feed.Content);
            Assert.Equal("new.png", feed.ImageUrl);
            Assert.Equal(newUrl, feed.VideoUrl);
            _feedRepositoryMock.Verify(r => r.UpdateAsync(feed, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task HandleImageFeed_ShouldThrow_WhenWrongType()
        {
            // Arrange
            var feed = new Domain.Feed { Id = 1, AuthorId = 1 };
            var request = new UpdateImageFeedCommand { Id = 1, UserId = 1, Title = "New", ImageUrl = "new.png" };

            _feedRepositoryMock.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>()))
                               .ReturnsAsync(feed);

            // Act & Assert
            await Assert.ThrowsAsync<BadRequestException>(() => _handler.Handle(request, CancellationToken.None));
        }
    }
}
