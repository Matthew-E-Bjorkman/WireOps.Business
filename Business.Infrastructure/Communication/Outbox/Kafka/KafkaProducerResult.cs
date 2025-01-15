namespace WireOps.Business.Infrastructure.Communication.Outbox.Kafka;

public enum KafkaProducerResult
{
    NoError,
    InvalidMessage,
    NoAck,
    OtherError
}