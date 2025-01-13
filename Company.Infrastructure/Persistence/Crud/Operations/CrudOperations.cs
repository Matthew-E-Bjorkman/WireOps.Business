using WireOps.Company.Infrastructure.Persistence.Crud.Results;

namespace WireOps.Company.Infrastructure.Persistence.Crud.Operations;

public interface CRUDOperations
{
    IQueryable<TEntity> Entities<TEntity>()
    where TEntity : class;

    Task<bool> CheckIfExists<TEntity>(Guid id)
        where TEntity : CrudEntity;

    Task<Created<TEntity>> Create<TEntity>(TEntity entity)
        where TEntity : CrudEntity;

    Task<TEntity> Read<TEntity>(Guid id)
        where TEntity : CrudEntity;

    Task<TEntity> Read<TEntity>(Guid id, QueryConfig<TEntity> queryConfig)
        where TEntity : CrudEntity;

    Task<TResult> Read<TEntity, TResult>(Guid id, QueryConfig<TEntity, TResult> queryConfig)
        where TEntity : CrudEntity;

    IAsyncEnumerable<TEntity> Read<TEntity>(QueryConfig<TEntity> queryConfig)
        where TEntity : class;

    IAsyncEnumerable<TResult> Read<TEntity, TResult>(QueryConfig<TEntity, TResult> queryConfig)
        where TEntity : class;

    Task<Updated<TEntity>> Update<TEntity>(Guid id, TEntity entity)
        where TEntity : CrudEntity;

    Task<Updated<TEntity>> Update<TEntity>(Guid id, QueryConfig<TEntity> queryConfig, TEntity entity)
        where TEntity : CrudEntity;

    //Task<Updated<TEntity>> Update<TEntity>(Guid id, JsonPatchDocument<TEntity> patch)
    //    where TEntity : CrudEntity;

    //Task<Updated<TEntity>> Update<TEntity>(Guid id, QueryConfig<TEntity> queryConfig,
    //    JsonPatchDocument<TEntity> patch)
    //    where TEntity : CrudEntity;

    Task<Updated> Update<TEntity>(Guid id, Action<TEntity> updateEntity)
        where TEntity : CrudEntity;

    Task<Updated> Update<TEntity>(Guid id, QueryConfig<TEntity> queryConfig, Action<TEntity> updateEntity)
        where TEntity : CrudEntity;

    Task<Updated<TEntity>> Update<TEntity>(Guid id, Func<TEntity, TEntity> updateEntity)
        where TEntity : CrudEntity;

    Task<Updated<TEntity>> Update<TEntity>(Guid id, QueryConfig<TEntity> queryConfig,
        Func<TEntity, TEntity> updateEntity)
        where TEntity : CrudEntity;

    Task<Updated<TResult>> Update<TEntity, TResult>(Guid id, Func<TEntity, TResult> updateEntity)
        where TEntity : CrudEntity
        where TResult : class;

    Task<Updated<TResult>> Update<TEntity, TResult>(Guid id, QueryConfig<TEntity> queryConfig,
        Func<TEntity, TResult> updateEntity)
        where TEntity : CrudEntity
        where TResult : class;

    Task<Deleted> Delete<TEntity>(Guid id, DeletePolicy deletePolicy)
        where TEntity : CrudEntity;

    Task SaveChanges();
}
