namespace WireOps.Company.Infrastructure.Communication.Outbox.Common;

public enum BatchProcessingResult
{
    NotFullBatchProcessed,
    FullBatchProcessed,
    TemporaryError
}