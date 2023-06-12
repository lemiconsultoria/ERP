using ERP.Consolidator.Domain.Entities;
using ERP.Core.Interfaces;

namespace ERP.Consolidator.Domain.Interfaces.Repositories
{
    public interface IEntryRepositoryBase<T> : IRepository<T> where T : Entry
    {
        T GetById(long id);

        decimal SumAllByDate(DateOnly dateRef);
    }
}