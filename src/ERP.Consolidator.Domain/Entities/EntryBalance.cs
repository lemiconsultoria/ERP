namespace ERP.Consolidator.Domain.Entities
{
    public class EntryBalance
    {
        public decimal TotalDebit { get; set; }
        public decimal TotalCredit { get; set; }
        public decimal Total { get; set; }
        public Guid Id { get; set; }
        public DateOnly ReferenceDate { get; set; }

        public EntryBalance()
        {
            Id = Guid.NewGuid();
        }
    }
}