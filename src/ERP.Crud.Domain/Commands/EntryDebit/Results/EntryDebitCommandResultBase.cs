using ERP.Core.Commands;

namespace ERP.Crud.Domain.Commands.EntryDebit.Results
{
    public class EntryDebitCommandResultBase : CommandDataResult
    {
        public long Id { get; set; }
        public string? Description { get; set; }
        public decimal Value { get; set; }
        public DateTime DateOfIssue { get; set; }
    }
}