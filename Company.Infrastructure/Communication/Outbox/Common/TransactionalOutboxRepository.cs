namespace WireOps.Company.Infrastructure.Communication.Outbox.Common;

public interface TransactionalOutboxRepository
{
    Task Save(IEnumerable<OutboxMessage> messages);
}