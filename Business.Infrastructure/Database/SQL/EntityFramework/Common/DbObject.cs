using NodaTime;

namespace Business.Infrastructure.Database.SQL.EntityFramework.Common;

public class DbObject
{
    public Instant CreatedAt { get; set; }
    public Instant ModifiedAt { get; set; }
}
