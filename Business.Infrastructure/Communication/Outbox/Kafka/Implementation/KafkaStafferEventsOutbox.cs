using WireOps.Business.Domain.Common.Definitions;
using WireOps.Business.Domain.Staffers.Events;
using WireOps.Business.Infrastructure.Communication.Outbox.Common;

namespace WireOps.Business.Infrastructure.Communication.Outbox.Kafka.Implementation;

public class KafkaStafferEventsOutbox(TransactionalOutboxes outboxes, TransactionalOutboxRepository repository,
    MessageTypes messageTypes) : TransactionalKafkaOutbox<StafferEvent>(outboxes, repository, messageTypes), StafferEventsOutbox
{
    protected override string Topic => "StafferEvents";

    protected override string GetPartitionKeyFor(StafferEvent message) => message.StafferId.ToString("N");
}
