using ERP.Core.Interfaces;
using ERP.Crud.Domain.Entities;

namespace ERP.Crud.Domain.Interfaces.Repositories
{
    public interface IEntryDebitRepository : IRepository<EntryDebit>, IRepositoryQuery<EntryDebit>, IRepositoryTransaction
    {
    }
}