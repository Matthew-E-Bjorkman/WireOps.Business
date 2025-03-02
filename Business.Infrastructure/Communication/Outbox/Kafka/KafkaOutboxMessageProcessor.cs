using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using WireOps.Business.Infrastructure.Communication.Outbox.Common;

namespace WireOps.Business.Infrastructure.Communication.Outbox.Kafka;

public class KafkaOutboxMessageProcessor(
    KafkaMessageProducer kafkaMessageProducer,
    ILogger<KafkaOutboxMessageProcessor> logger)
    : OutboxMessageProcessor
{
    public string ProcessorType => OutboxMessageProcessors.Kafka;

    public async Task<MessageProcessingResult> Process(OutboxMessage outboxMessage,
        CancellationToken cancellationToken)
    {
        if (!CheckProcessorTypeFor(outboxMessage))
            return MessageProcessingResult.MessageUnprocessable;
        if (!TryDeserialize(outboxMessage, out var kafkaMessage))
            return MessageProcessingResult.MessageUnprocessable;
        return await Produce(kafkaMessage, cancellationToken);
    }

    private bool CheckProcessorTypeFor(OutboxMessage outboxMessage)
    {
        if (outboxMessage.ProcessorType == ProcessorType)
            return true;
        logger.LogCritical(
            "Wrong OutboxMessageProcessor. Message {MessageId}, actual processor: {ActualProcessorType}, required processor: {RequiredProcessorType}",
            outboxMessage.Id, ProcessorType, outboxMessage.ProcessorType);
        return false;
    }

    private bool TryDeserialize(OutboxMessage outboxMessage, out KafkaMessage kafkaMessage)
    {
        try
        {
#pragma warning disable CS8601 // Possible null reference assignment.
            kafkaMessage = JsonConvert.DeserializeObject<KafkaMessage>(outboxMessage.PayloadAsJson);
#pragma warning restore CS8601 // Possible null reference assignment.
            return true;
        }
        catch (JsonException e)
        {
            logger.LogCritical(e, "Message deserialization failed. Message id: {MessageId}", outboxMessage.Id);
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
            kafkaMessage = null;
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
            return false;
        }
    }

    private async Task<MessageProcessingResult> Produce(KafkaMessage kafkaMessage,
        CancellationToken cancellationToken)
    {
        var result = await kafkaMessageProducer.Produce(kafkaMessage, cancellationToken);
        return result switch
        {
            KafkaProducerResult.NoError => MessageProcessingResult.Processed,
            KafkaProducerResult.InvalidMessage => MessageProcessingResult.MessageUnprocessable,
            KafkaProducerResult.NoAck => MessageProcessingResult.TemporaryError,
            KafkaProducerResult.OtherError => MessageProcessingResult.TemporaryError,
            _ => throw new ArgumentOutOfRangeException(nameof(result))
        };
    }
}