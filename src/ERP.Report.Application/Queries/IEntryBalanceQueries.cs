using ERP.Report.Domain.DTOs;

namespace ERP.Report.Application.Queries
{
    public interface IEntryBalanceQueries
    {
        Task<EntryBalanceDTO> GetByDateAsync(DateOnly dateRef);

        Task<IList<EntryBalanceDTO>> GetByPeriodAsync(DateOnly dateStart, DateOnly dateEnd);
    }
}