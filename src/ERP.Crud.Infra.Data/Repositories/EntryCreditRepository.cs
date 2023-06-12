using ERP.Crud.Domain.Entities;
using ERP.Crud.Domain.Interfaces.Repositories;
using ERP.Crud.Infra.Data.Context;
using ERP.Crud.Infra.Data.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace ERP.Crud.Infra.Data.Repositories
{
    public class EntryCreditRepository : RepositoryBase<EntryCredit>, IEntryCreditRepository
    {
        public EntryCreditRepository(MySQLContext _context) : base(_context)
        {
        }

        public async Task<EntryCredit> GetByIdAsync(long id)
        {
            return await _mySQLContext.Entries.OfType<EntryCredit>().AsNoTracking().FirstOrDefaultAsync(x => x.Id == id) ?? new EntryCredit();
        }

        public async Task<IEnumerable<EntryCredit>> GetAllAsync()
        {
            return await _mySQLContext.Entries.OfType<EntryCredit>().ToListAsync();
        }
    }
}