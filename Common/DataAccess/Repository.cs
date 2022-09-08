using Common.Negocio;

namespace Common.DataAccess;

public class Repository<T> : IRepository<T> where T : Entity
{
    private readonly UnitOfWork _context;

    public Repository(UnitOfWork context)
    {
        _context = context;
    }

    public IQueryable<T> Query()
    {
        return _context.Query<T>();
    }

    public T? Get(int id)
    {
        return _context.Get<T>(id);
    }

    public void Save(T entity)
    {
        _context.Save(entity);
    }

    public void Delete(T entity)
    {
        _context.Delete(entity);
    }

    public void SaveAll(IEnumerable<T> entities)
    {
        foreach (var entity in entities)
        {
            this.Save(entity);
        }
    }

    public void Commit()
    {
        this._context.Commit();
    }
}