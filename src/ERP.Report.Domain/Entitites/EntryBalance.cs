namespace ERP.Report.Domain.Entitites
{
    public class EntryBalance
    {
        public decimal TotalDebit { get; set; }
        public decimal TotalCredit { get; set; }
        public decimal Total { get; set; }
        public DateOnly ReferenceDate { get; set; }
        public Guid Id { get; set; }
    }
}