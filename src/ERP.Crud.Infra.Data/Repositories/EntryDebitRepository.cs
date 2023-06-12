using ERP.Crud.Domain.Entities;
using ERP.Crud.Domain.Interfaces.Repositories;
using ERP.Crud.Infra.Data.Context;
using ERP.Crud.Infra.Data.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace ERP.Crud.Infra.Data.Repositories
{
    public class EntryDebitRepository : RepositoryBase<EntryDebit>, IEntryDebitRepository
    {
        public EntryDebitRepository(MySQLContext _context) : base(_context)
        {
        }

        public async Task<EntryDebit> GetByIdAsync(long id)
        {
            return await _mySQLContext.Entries.OfType<EntryDebit>().FirstOrDefaultAsync(x => x.Id == id) ?? new EntryDebit();
        }

        public async Task<IEnumerable<EntryDebit>> GetAllAsync()
        {
            return await _mySQLContext.Entries.OfType<EntryDebit>().ToListAsync();
        }
    }
}