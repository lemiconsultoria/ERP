using ERP.Core.Commands;

namespace ERP.Crud.Domain.Commands.EntryCredit
{
    public class EntryCreditCommandBase : CommandBase
    {
        public string? Description { get; set; }
        public decimal Value { get; set; }
        public DateTime DateOfIssue { get; set; }
    }
}