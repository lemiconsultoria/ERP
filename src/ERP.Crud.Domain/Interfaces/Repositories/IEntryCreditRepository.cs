using ERP.Core.Interfaces;
using ERP.Crud.Domain.Entities;

namespace ERP.Crud.Domain.Interfaces.Repositories
{
    public interface IEntryCreditRepository : IRepository<EntryCredit>, IRepositoryQuery<EntryCredit>, IRepositoryTransaction
    {
    }
}