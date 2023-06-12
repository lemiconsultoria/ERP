using ERP.Core.Entities;

namespace ERP.Core.Interfaces
{
    public interface IRepositoryQuery<T> where T : IEntity
    {
        Task<T> GetByIdAsync(long id);

        Task<IEnumerable<T>> GetAllAsync();
    }
}