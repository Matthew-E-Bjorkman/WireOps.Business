using WireOps.Business.Domain.Common.Definitions;
using WireOps.Business.Domain.Companies.Events;
using WireOps.Business.Infrastructure.Communication.Outbox.Common;

namespace WireOps.Business.Infrastructure.Communication.Outbox.Kafka.Implementation;

public class KafkaCompanyEventsOutbox(TransactionalOutboxes outboxes, TransactionalOutboxRepository repository,
    MessageTypes messageTypes) : TransactionalKafkaOutbox<CompanyEvent>(outboxes, repository, messageTypes), CompanyEventsOutbox
{
    protected override string Topic => "CompanyEvents";

    protected override string GetPartitionKeyFor(CompanyEvent message) => message.CompanyId.ToString("N");
}
