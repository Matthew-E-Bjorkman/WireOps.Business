namespace WireOps.Company.Infrastructure.Communication.Outbox.Common;

public interface TransactionalOutboxProcessor
{
    Task<BatchProcessingResult> ProcessSingleBatch(int partition, CancellationToken cancellationToken);
}