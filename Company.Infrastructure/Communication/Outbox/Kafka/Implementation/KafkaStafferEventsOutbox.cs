using WireOps.Company.Domain.Common.Definitions;
using WireOps.Company.Domain.Staffers.Events;
using WireOps.Company.Infrastructure.Communication.Outbox.Common;

namespace WireOps.Company.Infrastructure.Communication.Outbox.Kafka.Implementation;

public class KafkaStafferEventsOutbox(TransactionalOutboxes outboxes, TransactionalOutboxRepository repository,
    MessageTypes messageTypes) : TransactionalKafkaOutbox<StafferEvent>(outboxes, repository, messageTypes), StafferEventsOutbox
{
    protected override string Topic => "StafferEvents";

    protected override string GetPartitionKeyFor(StafferEvent message) => message.StafferId.ToString("N");
}
