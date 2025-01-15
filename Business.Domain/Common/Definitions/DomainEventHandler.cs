using WireOps.Business.Common.Errors;

namespace WireOps.Business.Domain.Common.Definitions;

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