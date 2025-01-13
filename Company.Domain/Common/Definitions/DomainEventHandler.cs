using WireOps.Company.Common.Errors;

namespace WireOps.Company.Domain.Common.Definitions;

public interface DomainEventHandler<in TEvent> : MessageHandler
    where TEvent : DomainEvent
{
    Task MessageHandler.Handle(Message message)
    {
        if (!(message is TEvent domainEvent))
            throw new DesignError($"{message.GetType().Name} in incompatible with {GetType().Name}");
        return Handle(domainEvent);
    }
        
    Task Handle(TEvent domainEvent);
}