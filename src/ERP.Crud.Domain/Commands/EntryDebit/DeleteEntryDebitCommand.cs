using ERP.Core.Commands;

namespace ERP.Crud.Domain.Commands.EntryDebit
{
    public class DeleteEntryDebitCommand : CommandBase
    {
        public long Id { get; set; }
    }
}