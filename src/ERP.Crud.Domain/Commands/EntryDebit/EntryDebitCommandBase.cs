using ERP.Core.Commands;

namespace ERP.Crud.Domain.Commands.EntryDebit
{
    public class EntryDebitCommandBase : CommandBase
    {
        public string? Description { get; set; }
        public decimal Value { get; set; }
        public DateTime DateOfIssue { get; set; }
    }
}