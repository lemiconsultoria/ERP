using ERP.Crud.Domain.Entities;

namespace ERP.Crud.Application.Queries
{
    public interface IEntryCreditQueries
    {
        Task<IEnumerable<EntryCredit>> GetAllAsync();

        Task<EntryCredit> GetByIdAsync(long id);
    }
}