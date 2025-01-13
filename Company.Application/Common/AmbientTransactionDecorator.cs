using System.Transactions;

namespace WireOps.Company.Application.Common;

public class AmbientTransactionDecorator<TCommand>(CommandHandler<TCommand> decorated) : CommandHandler<TCommand>
    where TCommand : struct, Command
{
    public async Task Handle(TCommand command)
    {
        using var scope = new TransactionScope(
            TransactionScopeOption.RequiresNew,
            new TransactionOptions {IsolationLevel = IsolationLevel.ReadCommitted},
            TransactionScopeAsyncFlowOption.Enabled);
        await decorated.Handle(command);
        scope.Complete();
    }
}
    
public class AmbientTransactionDecorator<TCommand, TResult>(CommandHandler<TCommand, TResult> decorated)
    : CommandHandler<TCommand, TResult>
    where TCommand : struct, Command
{
    public async Task<TResult> Handle(TCommand command)
    {
        using var scope = new TransactionScope(
            TransactionScopeOption.RequiresNew,
            new TransactionOptions {IsolationLevel = IsolationLevel.ReadCommitted},
            TransactionScopeAsyncFlowOption.Enabled);
        var result = await decorated.Handle(command);
        scope.Complete();
        return result;
    }
}