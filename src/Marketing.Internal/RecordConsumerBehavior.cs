namespace Marketing.Internal
{
    using NServiceBus;

    public class RecordConsumerBehavior : ICommand
    {
        public string ProductId { get; set; }
    }
}