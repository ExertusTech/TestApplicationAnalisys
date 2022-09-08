namespace Utility.Negocio;

public abstract class AggregateRoot : Entity, IAuditable
{

    protected AggregateRoot()
    {

    }

    protected AggregateRoot(int id):base(id)
    {
    }

    public virtual string AuditUser { get; set; } = "";

    public virtual DateTime AuditDate { get; set; }

    private readonly List<IDomainEvent> _domainEvents = new List<IDomainEvent>();
    public virtual IReadOnlyList<IDomainEvent> DomainEvents => _domainEvents;

    protected virtual void AddDomainEvent(IDomainEvent newEvent)
    {
        _domainEvents.Add(newEvent);
    }

    public virtual void ClearEvents()
    {
        _domainEvents.Clear();
    }
}

public interface IAuditable
{
    DateTime AuditDate { get; set; }

    string AuditUser { get; set; }
}