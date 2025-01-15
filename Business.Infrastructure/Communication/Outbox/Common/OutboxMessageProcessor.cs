namespace WireOps.Business.Infrastructure.Communication.Outbox.Common;

public interface OutboxMessageProcessor
{
    string ProcessorType { get; }
    Task<MessageProcessingResult> Process(OutboxMessage outboxMessage, CancellationToken cancellationToken);
}