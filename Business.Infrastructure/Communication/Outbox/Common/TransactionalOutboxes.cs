namespace WireOps.Business.Infrastructure.Communication.Outbox.Common;

public class TransactionalOutboxes
{
    private readonly List<TransactionalOutbox> _outboxes = new();
    public IReadOnlyList<TransactionalOutbox> ForCurrentUseCase => _outboxes.AsReadOnly();

    public void Register(TransactionalOutbox outbox) => _outboxes.Add(outbox);
}