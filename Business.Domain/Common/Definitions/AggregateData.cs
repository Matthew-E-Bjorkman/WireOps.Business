using NodaTime;

namespace Business.Domain.Common.Definitions;

public interface AggregateData
{
    public Instant CreatedAt { get; }
    public Instant ModifiedAt { get; }
}
