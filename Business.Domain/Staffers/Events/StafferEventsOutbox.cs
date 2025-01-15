namespace WireOps.Business.Domain.Staffers.Events;
public interface StafferEventsOutbox
{
    public void Add(StafferEvent orderEvent);
}