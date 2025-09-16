using AttrectoTest.Application.Features.Feed.Commands.PurgeFeed;

using MediatR;

using Quartz;


namespace AttrectoTest.ApiService.Jobs;

public class PurgeFeedsJob(IMediator mediator) : IJob
{
    public async Task Execute(IJobExecutionContext context)
    {
        var command = new PurgeFeedCommand();
        await mediator.Send(command);
    }
}
