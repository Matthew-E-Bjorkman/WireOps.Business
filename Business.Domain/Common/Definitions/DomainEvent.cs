using NodaTime;

namespace WireOps.Business.Domain.Common.Definitions;

public interface DomainEvent : Message 
{
    public Instant EventCreatedAt { get; } 
}