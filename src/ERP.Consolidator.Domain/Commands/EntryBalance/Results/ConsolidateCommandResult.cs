using ERP.Core.Commands;

namespace ERP.Consolidator.Domain.Commands.EntryBalance
{
    public class ConsolidateCommandResult : CommandDataResult
    {
        public Dictionary<DateOnly, bool> DatesProcessed { get; set; }

        public ConsolidateCommandResult()
        {
            DatesProcessed = new Dictionary<DateOnly, bool>();
        }
    }
}