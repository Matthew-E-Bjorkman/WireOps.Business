﻿using NodaTime;
using NodaTime.Extensions;

namespace WireOps.Business.Domain.Roles.Events;

public class RoleDeleted(Guid companyId, Guid roleId) : RoleEvent
{
    public Guid CompanyId { get; } = companyId;
    public Guid RoleId { get; } = roleId;
    public Instant EventCreatedAt { get; } = DateTime.UtcNow.ToInstant();
}