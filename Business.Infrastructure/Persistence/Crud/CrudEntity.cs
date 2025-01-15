using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace WireOps.Business.Infrastructure.Persistence.Crud;

public class CrudEntity
{
    [BindNever] public Guid Id { get; set; }
    [BindNever] public bool IsDeleted { get; set; }
}
