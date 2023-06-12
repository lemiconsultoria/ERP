using ERP.Core.DTOs;

namespace ERP.Report.Domain.DTOs
{
    public class EntryBalanceDTO : DTOBase
    {
        public decimal TotalDebit { get; set; }
        public decimal TotalCredit { get; set; }
        public decimal Total { get; set; }
        public DateOnly ReferenceDate { get; set; }
    }
}