namespace WireOps.Business.Infrastructure.Persistence.Crud.Operations;

public delegate IQueryable<T> QueryConfig<T>(IQueryable<T> queryable);

public delegate IQueryable<TOut> QueryConfig<in TIn, out TOut>(IQueryable<TIn> queryable);