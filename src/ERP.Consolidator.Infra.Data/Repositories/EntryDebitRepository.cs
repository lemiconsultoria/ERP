using ERP.Consolidator.Domain.Entities;
using ERP.Consolidator.Domain.Interfaces.Repositories;
using ERP.Consolidator.Infra.Data.Context;
using ERP.Consolidator.Infra.Data.Repositories.Base;

namespace ERP.Consolidator.Infra.Data.Repositories
{
    public class EntryDebitRepository : EntryRepositoryBase<EntryDebit>, IEntryDebitRepository
    {
        public EntryDebitRepository(MongoDBContext _context) : base(_context)
        {
        }
    }
}