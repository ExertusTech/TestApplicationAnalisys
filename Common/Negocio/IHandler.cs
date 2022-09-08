namespace Common.Negocio;

public interface IHandler<in T>
    where T : IDomainEvent
{
    void Handle(T domainEvent);
}