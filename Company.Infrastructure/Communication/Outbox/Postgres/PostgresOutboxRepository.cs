using WireOps.Company.Infrastructure.Communication.Outbox.Common;
using WireOps.Company.Infrastructure.Database.SQL.EntityFramework;

namespace WireOps.Company.Infrastructure.Communication.Outbox.Postgres;

public class PostgresOutboxRepository(CompanyDbContext dbContext) : TransactionalOutboxRepository
{
    private readonly CompanyDbContext _dbContext = dbContext;

    public Task Save(IEnumerable<OutboxMessage> messages)
    {
        throw new NotImplementedException();
    }

    public async Task<Batch> GetUnprocessedMessagesFor(int partition, int batchSize,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public async Task SaveCurrentOffset(int partition, long currentOffset)
    {
        throw new NotImplementedException();
    }

    public async Task MoveToUnprocessableMessages(OutboxMessage message)
    {
        throw new NotImplementedException();
    }
}