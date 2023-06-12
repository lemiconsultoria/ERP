using ERP.Crud.Domain.Entities;
using ERP.Crud.Domain.Interfaces.Repositories;

namespace ERP.Crud.Application.Queries
{
    public class EntryCreditQueries : IEntryCreditQueries
    {
        private readonly IEntryCreditRepository _entryCreditRepository;

        public EntryCreditQueries(IEntryCreditRepository entryCreditRepository)
        {
            _entryCreditRepository = entryCreditRepository;
        }

        public async Task<IEnumerable<EntryCredit>> GetAllAsync()
        {
            return await _entryCreditRepository.GetAllAsync();
        }

        public async Task<EntryCredit> GetByIdAsync(long id)
        {
            return await _entryCreditRepository.GetByIdAsync(id);
        }
    }
}