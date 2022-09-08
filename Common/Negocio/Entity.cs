using NHibernate.Proxy;

namespace Common.Negocio;

public abstract class Entity:IEntity
{
        
    public virtual int Id { get;  set; }

    protected Entity()
    {
    }

    protected Entity(int id) : this()
    {
        Id = id;
    }

    public override bool Equals(object? obj)
    {
        if (!(obj is Entity other))
            return false;

        if (ReferenceEquals(this, other))
            return true;

        if (GetRealType() != other.GetRealType())
            return false;

        if (Id == 0 || other.Id == 0)
            return false;

        return Id == other.Id;
    }

#pragma warning disable S3875 // "operator==" Solo en esta entidad ya que tomaremos en 
    public static bool operator ==(Entity? a, Entity? b)
#pragma warning restore S3875 // "operator==" should not be overloaded on reference types
    {
        if (a is null && b is null)
            return true;

        if (a is null || b is null)
            return false;

        return a.Equals(b);
    }

    public static bool operator !=(Entity? a, Entity? b)
    {
        return !(a == b);
    }

    public override int GetHashCode()
    {
        return (GetRealType().ToString() + Id).GetHashCode();
    }

    private Type GetRealType()
    {
        return NHibernateProxyHelper.GetClassWithoutInitializingProxy(this);
    }
}