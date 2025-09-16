using AttrectoTest.Application.Contracts.Identity;
using AttrectoTest.Domain;
using AttrectoTest.Persistence.DatabaseContext;

using Microsoft.EntityFrameworkCore;


namespace AttrectoTest.Persistence.Seed;

internal class DbSeeder(TestDbContext dbContext, IAppUserService userService)
{
    public async Task SeedAsync()
    {
        if (await dbContext.Feeds.AnyAsync() || await dbContext.AppUsers.AnyAsync())
        {
            return; // DB has been seeded
        }
        var users = new List<(string UserName, string Password, string Role)>
        {
            ("alice", "Passw0rd!", "User"),
            ("bob", "Passw0rd!", "User"),
            ("admin", "AdminPassw0rd!", "Admin")
        };
        
        foreach (var (userName, password, role) in users)
        {
            await userService.AddNewUser(userName, password, role);
        }
        var Alice = await dbContext.AppUsers.FirstAsync(u => u.UserName == "alice");
        var Bob = await dbContext.AppUsers.FirstAsync(u => u.UserName == "bob");


        var feed1 = new Feed { Title = "Hello World", Content = "This is the very first feed in the system. Welcome aboard!", Author = Alice };
        var feed2 = new Feed { Title = "Daily Motivation", Content = "Every day is a new opportunity to grow and improve yourself.", Author = Bob };
        var feed3 = new Feed { Title = "Tech News", Content = "Microsoft announces new features in .NET 8 for developers worldwide.", Author = Alice };
        var feed4 = new ImageFeed { Title = "Photo of the Day", Content = "Check out this beautiful sunset I captured last night.", Author = Bob, ImageUrl = image1 };
        var feed5 = new Feed { Title = "Cooking Tips", Content = "Add a pinch of salt to your coffee to reduce bitterness.", Author = Alice };
        var feed6 = new Feed { Title = "Book Recommendation", Content = "You should definitely read 'Clean Code' by Robert C. Martin.", Author = Bob };
        var feed7 = new Feed { Title = "Workout Routine", Content = "Today I tried a 20-minute HIIT session and it was super effective.", Author = Alice };
        var feed8 = new Feed { Title = "Music Vibes", Content = "Listening to jazz while coding helps me focus better.", Author = Bob };
        var feed9 = new Feed { Title = "Weekend Plans", Content = "Planning to go hiking in the mountains this Saturday.", Author = Alice };
        var feed10 = new VideoFeed { Title = "Movie Night", Content = "Watched 'Inception' again. Still a masterpiece!", Author = Bob, ImageUrl = image3, VideoUrl = "https://example.com/videos/inception.mp4" };
        var feed11 = new Feed { Title = "Coding Challenge", Content = "Can you solve this algorithm problem in under 5 minutes?", Author = Alice };
        var feed12 = new Feed { Title = "Travel Goals", Content = "Dreaming of visiting Japan during the cherry blossom season.", Author = Bob };
        var feed13 = new Feed { Title = "Pet Story", Content = "My dog just learned a new trick: rolling over on command.", Author = Alice };
        var feed14 = new Feed { Title = "Life Hack", Content = "Use binder clips to organize your cables at the desk.", Author = Bob };
        var feed15 = new Feed { Title = "Coffee Time", Content = "Started the day with a strong espresso. Ready to code!", Author = Alice };
        var feed16 = new ImageFeed { Title = "Gardening Update", Content = "The tomatoes in my backyard are finally turning red.", Author = Bob, ImageUrl = image2 };
        var feed17 = new Feed { Title = "Inspirational Quote", Content = "Success is not final, failure is not fatal: it is the courage to continue that counts.", Author = Alice };
        var feed18 = new Feed { Title = "Learning Journal", Content = "Today I learned about CQRS and how it helps separate concerns.", Author = Bob };
        var feed19 = new VideoFeed { Title = "Gaming Session", Content = "Spent 3 hours in Elden Ring and finally beat that boss!", Author = Alice, ImageUrl = image4,  VideoUrl = "https://example.com/videos/eldenring.mp4" };
        var feed20 = new Feed { Title = "DIY Project", Content = "Built a small wooden shelf for my books over the weekend.", Author = Bob };
        var feed21 = new Feed { Title = "Healthy Eating", Content = "Tried making a quinoa salad with roasted veggies. Delicious!", Author = Alice };
        var feed22 = new Feed { Title = "Weather Report", Content = "It’s sunny today but rain is expected tomorrow afternoon.", Author = Bob };
        var feed23 = new Feed { Title = "Fun Fact", Content = "Did you know octopuses have three hearts?", Author = Alice };
        var feed24 = new Feed { Title = "Coding Tip", Content = "Always write unit tests for your critical business logic.", Author = Bob };
        var feed25 = new VideoFeed { Title = "Weekend Reflection", Content = "Took a break from coding and spent quality time with family.", Author = Alice, ImageUrl = image5, VideoUrl = "https://example.com/videos/family.mp4" };

        await dbContext.Feeds.AddRangeAsync(feed1, feed2, feed3, feed4, feed5, feed6, feed7, feed8, feed9, feed10, feed11, feed12, feed13, feed14, feed15, feed16, feed17, feed18, feed19, feed20, feed21, feed22, feed23, feed24, feed25);

        var like1 = new FeedLike { Feed = feed1, User = Bob };
        var like2 = new FeedLike { Feed = feed2, User = Alice };
        var like6 = new FeedLike { Feed = feed2, User = Bob };
        var like3 = new FeedLike { Feed = feed3, User = Bob };
        var like4 = new FeedLike { Feed = feed4, User = Alice };
        var like5 = new FeedLike { Feed = feed5, User = Bob };

        await dbContext.FeedLikes.AddRangeAsync(like1, like2, like3, like4, like5, like6);

        var comment1 = new Comment { Feed = feed1, User = Bob, Content = "Great first post, Alice! Looking forward to more." };
        var comment2 = new Comment { Feed = feed1, User = Alice, Content = "Thanks for the motivation, Bob! Needed that today." };
        var comment3 = new Comment { Feed = feed1, User = Bob, Content = "Exciting news! .NET 8 is going to be a game-changer." };
        var comment4 = new Comment { Feed = feed1, User = Alice, Content = "Amazing photo, Bob! The colors are stunning." };
        var comment5 = new Comment { Feed = feed1, User = Bob, Content = "Interesting tip! I'll try that next time I make coffee." };
        await dbContext.Comments.AddRangeAsync(comment1, comment2, comment3, comment4, comment5);

        await dbContext.SaveChangesAsync();
    }

    private const string image1 = "/seed/flower1.jpg";
    private const string image2 = "/seed/flower2.jpg";
    private const string image3 = "/seed/flower3.jpg";
    private const string image4 = "/seed/flower4.jpg";
    private const string image5 = "/seed/flower5.jpg";
}
