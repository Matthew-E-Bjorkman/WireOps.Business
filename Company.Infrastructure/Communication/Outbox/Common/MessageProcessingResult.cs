namespace WireOps.Company.Infrastructure.Communication.Outbox.Common;

public enum MessageProcessingResult
{
    Processed,
    TemporaryError,
    MessageUnprocessable
}