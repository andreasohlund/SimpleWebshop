namespace Marketing.Api.MessageHandlers;

using Marketing.Internal;
using NServiceBus;

public class RecordConsumerBehaviorHandler(ILogger<RecordConsumerBehaviorHandler> logger) : IHandleMessages<RecordConsumerBehavior>
{
    public async Task Handle(RecordConsumerBehavior message, IMessageHandlerContext context)
    {
        // Simulate some work
        await Task.Delay(Random.Shared.Next(25, 50), context.CancellationToken);

        logger.LogInformation("Perform marketing campaign.");
    }
}