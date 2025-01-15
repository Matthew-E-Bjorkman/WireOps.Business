namespace WireOps.Business.Infrastructure.Communication.Outbox.Postgres;

public class PostgresOutboxProcessorSettings
{
    public int BatchSize { get; set; }
    public int CommitOffsetInterval { get; set; }
    public int CleanupThreshold { get; set; }
}