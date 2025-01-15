namespace WireOps.Business.Domain.Common.Definitions;

public interface MessageHandler
{
    Task Handle(Message message);
}