using ERP.Core.AutoMapper;
using ERP.Core.Helpers;
using ERP.Report.Domain.DTOs;
using ERP.Report.Domain.Entitites;
using ERP.Report.Domain.Interfaces.Repositories;

namespace ERP.Report.Application.Queries
{
    public class EntryBalanceQueries : IEntryBalanceQueries
    {
        private readonly IEntryBalanceRepository _entryBalanceRepository;

        public EntryBalanceQueries(IEntryBalanceRepository entryBalanceRepository)
        {
            _entryBalanceRepository = entryBalanceRepository;
        }

        public async Task<EntryBalanceDTO> GetByDateAsync(DateOnly dateRef)
        {
            try
            {
                var entity = await _entryBalanceRepository.GetByDateAsync(dateRef);

                var result = Mapper.ClassToDTO<EntryBalance, EntryBalanceDTO>(entity);

                return result;
            }
            catch (Exception ex)
            {
                LogHelper.RiseError(ex);
                throw;
            }
        }

        public async Task<IList<EntryBalanceDTO>> GetByPeriodAsync(DateOnly dateStart, DateOnly dateEnd)
        {
            try
            {
                var balancesDays = new List<EntryBalanceDTO>();

                var entities = await _entryBalanceRepository.GetByPeriodAsync(dateStart, dateEnd);

                entities.ToList().ForEach(entity => balancesDays.Add(Mapper.ClassToDTO<EntryBalance, EntryBalanceDTO>(entity)));

                return balancesDays;
            }
            catch (Exception ex)
            {
                LogHelper.RiseError(ex);
                throw;
            }
        }
    }
}