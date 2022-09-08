namespace Common.Negocio;

public interface IEntity
{
    int Id { get;  }
}


public interface IEquivalentComparer<in T> : IEntity
{
    bool IsEquivalentTo(T compare);
}