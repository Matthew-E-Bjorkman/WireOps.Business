namespace WireOps.Business.Application.Common;

public interface QueryHandler<in TQuery, TResult> where TQuery : Query
{
    Task<TResult> Handle(TQuery query);
}
