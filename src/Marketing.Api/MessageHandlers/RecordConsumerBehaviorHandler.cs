namespace Marketing.Api.MessageHandlers;

using Marketing.Internal;
using NServiceBus;

public class RecordConsumerBehaviorHandler : IHandleMessages<RecordConsumerBehavior>
{
    static readonly Random random = new Random();

    readonly ILogger logger;

    public RecordConsumerBehaviorHandler(ILogger<RecordConsumerBehaviorHandler> logger)
    {
        this.logger = logger;
    }

    public async Task Handle(RecordConsumerBehavior message, IMessageHandlerContext context)
    {
        // Simulate some work
        await Task.Delay(random.Next(25, 50), context.CancellationToken);

        logger.LogInformation("Perform marketing campaign.");
    }
}