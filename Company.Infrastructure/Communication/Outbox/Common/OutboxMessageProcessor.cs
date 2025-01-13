namespace WireOps.Company.Infrastructure.Communication.Outbox.Common;

public interface OutboxMessageProcessor
{
    string ProcessorType { get; }
    Task<MessageProcessingResult> Process(OutboxMessage outboxMessage, CancellationToken cancellationToken);
}