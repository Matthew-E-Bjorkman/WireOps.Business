using WireOps.Business.Domain.Common.Definitions;
using WireOps.Business.Domain.Roles.Events;
using WireOps.Business.Infrastructure.Communication.Outbox.Common;

namespace WireOps.Business.Infrastructure.Communication.Outbox.Kafka.Implementation;

public class KafkaRoleEventsOutbox(TransactionalOutboxes outboxes, TransactionalOutboxRepository repository,
    MessageTypes messageTypes) : TransactionalKafkaOutbox<RoleEvent>(outboxes, repository, messageTypes), RoleEventsOutbox
{
    protected override string Topic => "RoleEvents";

    protected override string GetPartitionKeyFor(RoleEvent message) => message.RoleId.ToString("N");
}
