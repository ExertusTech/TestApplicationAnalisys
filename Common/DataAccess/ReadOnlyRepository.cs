using CSharpFunctionalExtensions;
using Entity = Common.Negocio.Entity;

namespace Common.DataAccess
{
    public class ReadOnlyRepository : IReadOnlyRepository
    {
        private readonly UnitOfWork _context;

        public ReadOnlyRepository(UnitOfWork context)
        {
            _context = context;
        }

        public IQueryable<T> Query<T>() where T : Entity
        {
            return _context.Query<T>();
        }

        public Result<T> Get<T>(int id) where T : Entity
        {
            var item = _context.Get<T>(id);
            return item ?? Result.Failure<T>("NotFound");
        }

        public T? GetOrDefault<T>(int id) where T : Entity
        {
            return _context.Get<T>(id);
        }
    }
}
