using CSharpFunctionalExtensions;
using Entity = Common.Negocio.Entity;

namespace Common.DataAccess
{
    public interface IReadOnlyRepository
    {
        IQueryable<T> Query<T>() where T : Entity;
        Result<T> Get<T>(int id) where T : Entity;
        T? GetOrDefault<T>(int id) where T : Entity;
    }
}