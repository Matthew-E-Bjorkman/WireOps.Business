namespace WireOps.Business.Domain.Companies.Events;
public interface CompanyEventsOutbox
{
    public void Add(CompanyEvent orderEvent);
}