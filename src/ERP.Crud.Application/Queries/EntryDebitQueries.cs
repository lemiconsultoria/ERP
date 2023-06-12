using ERP.Crud.Domain.Entities;
using ERP.Crud.Domain.Interfaces.Repositories;

namespace ERP.Crud.Application.Queries
{
    public class EntryDebitQueries : IEntryDebitQueries
    {
        private readonly IEntryDebitRepository _entryDebitRepository;

        public EntryDebitQueries(IEntryDebitRepository entryDebitRepository)
        {
            _entryDebitRepository = entryDebitRepository;
        }

        public async Task<IEnumerable<EntryDebit>> GetAllAsync()
        {
            return await _entryDebitRepository.GetAllAsync();
        }

        public async Task<EntryDebit> GetByIdAsync(long id)
        {
            return await _entryDebitRepository.GetByIdAsync(id);
        }
    }
}