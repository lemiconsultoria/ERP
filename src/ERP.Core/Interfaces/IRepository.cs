using ERP.Core.Entities;

namespace ERP.Core.Interfaces
{
    public interface IRepository<in T> where T : IEntity
    {
        void Add(T entity);

        void Update(T entity);

        void Delete(T entity);
    }
}