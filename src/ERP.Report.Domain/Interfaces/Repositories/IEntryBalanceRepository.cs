using ERP.Report.Domain.Entitites;

namespace ERP.Report.Domain.Interfaces.Repositories
{
    public interface IEntryBalanceRepository
    {
        Task<EntryBalance> GetByDateAsync(DateOnly dateRef);

        Task<IList<EntryBalance>> GetByPeriodAsync(DateOnly dateStart, DateOnly dateEnd);
    }
}