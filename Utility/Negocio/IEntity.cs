namespace Utility.Negocio
{
    public interface IEntity
    {
        int Id { get;  }
    }

    public interface ICustomCopyable<in T> : IEntity
    {
        void CustomCopy(T entity);
    }

    public interface IEquivalentComparer<in T> : IEntity
    {
        bool IsEquivalentTo(T compare);
    }
}
