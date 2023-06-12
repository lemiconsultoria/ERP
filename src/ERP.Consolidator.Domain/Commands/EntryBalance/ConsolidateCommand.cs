using ERP.Core.Commands;

namespace ERP.Consolidator.Domain.Commands.EntryBalance
{
    public class ConsolidateCommand : CommandBase
    {
        public bool OnlyToday { get; set; }
    }
}