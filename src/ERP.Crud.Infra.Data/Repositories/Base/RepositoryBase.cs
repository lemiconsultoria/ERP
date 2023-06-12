using ERP.Core.Entities;
using ERP.Core.Interfaces;
using ERP.Crud.Infra.Data.Context;

namespace ERP.Crud.Infra.Data.Repositories.Base
{
    public class RepositoryBase<T> : IRepository<T>, IRepositoryTransaction where T : IEntity
    {
        protected readonly MySQLContext _mySQLContext;

        public RepositoryBase(MySQLContext mySQLContext)
        {
            _mySQLContext = mySQLContext;
        }

        public int SaveChanges() => _mySQLContext.SaveChangesAsync().Result;

        public void Add(T entity) => _mySQLContext.Add(entity);

        public void Update(T entity) => _mySQLContext.Update(entity);

        public void Delete(T entity) => _mySQLContext.Remove(entity);
    }
}