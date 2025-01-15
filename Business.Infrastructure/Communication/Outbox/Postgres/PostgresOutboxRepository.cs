using WireOps.Business.Infrastructure.Communication.Outbox.Common;
using WireOps.Business.Infrastructure.Database.SQL.EntityFramework;

namespace WireOps.Business.Infrastructure.Communication.Outbox.Postgres;

public class PostgresOutboxRepository(BusinessDbContext dbContext) : TransactionalOutboxRepository
{
    private readonly BusinessDbContext _dbContext = dbContext;

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