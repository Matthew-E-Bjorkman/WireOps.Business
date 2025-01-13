using WireOps.Company.Domain.Common.Definitions;

namespace WireOps.Company.Application.Common;

public interface CommandHandler<in TCommand> : MessageHandler where TCommand : Command
{
    Task MessageHandler.Handle(Message message)
    {
        if (!(message is TCommand command))
        {
            throw new Exception($"{message.GetType().Name} in incompatible with {GetType().Name}");
        }
            
        return Handle(command);
    }

    Task Handle(TCommand command);
}

public interface CommandHandler<in TCommand, TResult> : CommandHandler<TCommand> where TCommand : Command
{
    Task CommandHandler<TCommand>.Handle(TCommand command) => Handle(command);

    new Task<TResult> Handle(TCommand command);
}
