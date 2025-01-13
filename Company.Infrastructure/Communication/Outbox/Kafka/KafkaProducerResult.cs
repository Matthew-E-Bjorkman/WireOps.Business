namespace WireOps.Company.Infrastructure.Communication.Outbox.Kafka;

public enum KafkaProducerResult
{
    NoError,
    InvalidMessage,
    NoAck,
    OtherError
}