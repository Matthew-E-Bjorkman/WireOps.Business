
using Microsoft.Extensions.DependencyInjection;
using WireOps.Business.Domain.Common.Definitions;

namespace WireOps.Business.Infrastructure.Communication.Publisher;

public class DomainEventPublisher
{
    private readonly IServiceProvider _serviceProvider;

    public DomainEventPublisher(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task PublishAsync<TEvent>(TEvent @event) where TEvent : DomainEvent
    {
        using var scope = _serviceProvider.CreateScope();
        var serviceProvider = scope.ServiceProvider;

        var handlerType = typeof(DomainEventHandler<>).MakeGenericType(@event.GetType());

        var handlers = serviceProvider.GetServices(handlerType);
        foreach (var handler in handlers)
        {
            var method = handler!.GetType().GetMethod("Handle");
            if (method != null)
            {
                var task = (Task)method.Invoke(handler, new object[] { @event })!;
                await task;
            }
        }
    }
}