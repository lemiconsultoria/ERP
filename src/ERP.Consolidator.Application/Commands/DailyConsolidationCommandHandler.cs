using ERP.Consolidator.Domain.Commands.EntryBalance;
using ERP.Consolidator.Domain.Entities;
using ERP.Consolidator.Domain.Interfaces.Repositories;
using ERP.Core.Commands;
using ERP.Core.Helpers;
using ERP.Core.Interfaces;

namespace ERP.Consolidator.Application.Commands
{
    public class DailyConsolidationCommandHandler : CommandHandlerBase<ConsolidateCommandResult>, ICommandHandler<ConsolidateCommand, ConsolidateCommandResult>
    {
        private readonly IEntryCreditRepository _entryCreditRepository;
        private readonly IEntryDebitRepository _entryDebitRepository;
        private readonly IDateToProcessRepository _dateToProcessDebitRepository;
        private readonly IEntryBalanceRepository _entryBalanceRepository;

        public DailyConsolidationCommandHandler(
            IEntryCreditRepository entryCreditRepositor,
            IEntryDebitRepository entryDebitRepository,
            IDateToProcessRepository dateToProcessDebitRepository,
            IEntryBalanceRepository entryBalanceRepository
            )
        {
            _entryCreditRepository = entryCreditRepositor;
            _entryDebitRepository = entryDebitRepository;
            _dateToProcessDebitRepository = dateToProcessDebitRepository;
            _entryBalanceRepository = entryBalanceRepository;
        }

        public CommandResult<ConsolidateCommandResult> Handle(ConsolidateCommand command)
        {
            try
            {
                IList<DateToProcess> daysToProcess = new List<DateToProcess>();

                if (command.OnlyToday)
                    daysToProcess.Add(new DateToProcess { ReferenceDate = DateOnly.FromDateTime(DateTime.Now) });
                else
                    daysToProcess = _dateToProcessDebitRepository.GetDatesToProcess();

                if (daysToProcess == null || !daysToProcess.Any())
                    return ReturnError("DaysToProcess is null");

                var result = ProcessDates(daysToProcess);
                return Return(result);
            }
            catch (Exception ex)
            {
                LogHelper.RiseError(ex);
                throw;
            }
        }

        private ConsolidateCommandResult ProcessDates(IList<DateToProcess> daysToProcess)
        {
            var result = new ConsolidateCommandResult();

            foreach (var dayToProcess in daysToProcess)
            {
                if (result.DatesProcessed.ContainsKey(dayToProcess.ReferenceDate)) continue;

                var totalDebits = _entryDebitRepository.SumAllByDate(dayToProcess.ReferenceDate);
                var totalCredits = _entryCreditRepository.SumAllByDate(dayToProcess.ReferenceDate);

                var entryBalanceEntity = new EntryBalance
                {
                    TotalDebit = totalDebits,
                    TotalCredit = totalCredits,
                    Total = totalDebits - totalCredits,
                    ReferenceDate = dayToProcess.ReferenceDate
                };

                var entityExists = _entryBalanceRepository.GetByDate(dayToProcess.ReferenceDate);

                if (entityExists != null && entityExists.ReferenceDate == dayToProcess.ReferenceDate)
                {
                    entryBalanceEntity.Id = entityExists.Id;
                    _entryBalanceRepository.Update(entryBalanceEntity);
                }
                else
                {
                    _entryBalanceRepository.Add(entryBalanceEntity);
                }

                _dateToProcessDebitRepository.RemoveDate(dayToProcess.ReferenceDate);

                result.DatesProcessed.Add(dayToProcess.ReferenceDate, true);
            }

            return result;
        }
    }
}