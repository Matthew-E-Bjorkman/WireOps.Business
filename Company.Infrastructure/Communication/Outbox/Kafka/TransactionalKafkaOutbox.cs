using Newtonsoft.Json;
using WireOps.Company.Domain.Common.Definitions;
using WireOps.Company.Infrastructure.Communication.Outbox.Common;

namespace WireOps.Company.Infrastructure.Communication.Outbox.Kafka;

public abstract class TransactionalKafkaOutbox<TMessage>(
    TransactionalOutboxes outboxes,
    TransactionalOutboxRepository repository,
    MessageTypes messageTypes)
    : TransactionalOutbox<TMessage>(outboxes, repository, messageTypes)
    where TMessage : Message
{
    protected abstract string Topic { get; }

    protected override string GetProcessorTypeFor(TMessage message) => OutboxMessageProcessors.Kafka;

    protected override string CreatePayloadFrom(TMessage message)
    {
        var kafkaMessage = new KafkaMessage(Topic,
            GetPartitionKeyFor(message),
            Serialize(message));
        return Serialize(kafkaMessage);
    }

    // TODO: flexible serialization (json, avro, etc. - Kafka specific)
    private static string Serialize(TMessage message) => JsonConvert.SerializeObject(message);

    private static string Serialize(KafkaMessage kafkaMessage) => JsonConvert.SerializeObject(kafkaMessage);
}