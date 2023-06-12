using ERP.Crud.Domain.Entities;

namespace ERP.Crud.Application.Queries
{
    public interface IEntryDebitQueries
    {
        Task<IEnumerable<EntryDebit>> GetAllAsync();

        Task<EntryDebit> GetByIdAsync(long id);
    }
}