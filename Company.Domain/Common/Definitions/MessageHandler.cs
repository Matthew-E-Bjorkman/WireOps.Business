namespace WireOps.Company.Domain.Common.Definitions;

public interface MessageHandler
{
    Task Handle(Message message);
}