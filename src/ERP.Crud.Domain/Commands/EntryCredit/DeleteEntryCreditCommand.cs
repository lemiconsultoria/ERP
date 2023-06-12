using ERP.Core.Commands;

namespace ERP.Crud.Domain.Commands.EntryCredit
{
    public class DeleteEntryCreditCommand : CommandBase
    {
        public long Id { get; set; }
    }
}