using ERP.Consolidator.Domain.Entities;

namespace ERP.Consolidator.Domain.Interfaces.Repositories
{
    public interface IEntryBalanceRepository
    {
        void Add(EntryBalance entity);

        void Update(EntryBalance entity);

        EntryBalance GetByDate(DateOnly dateRef);
    }
}