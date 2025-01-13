using NodaTime;

namespace WireOps.Company.Domain.Common.Definitions;

public interface DomainEvent : Message 
{
    public Instant EventCreatedAt { get; } 
}