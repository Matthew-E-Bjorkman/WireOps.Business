namespace WireOps.Business.Infrastructure.Communication.Outbox.Common;

public enum MessageProcessingResult
{
    Processed,
    TemporaryError,
    MessageUnprocessable
}