namespace WireOps.Business.Infrastructure.Communication.Outbox.Common;

public enum BatchProcessingResult
{
    NotFullBatchProcessed,
    FullBatchProcessed,
    TemporaryError
}