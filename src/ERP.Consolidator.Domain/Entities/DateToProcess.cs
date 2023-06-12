namespace ERP.Consolidator.Domain.Entities
{
    public class DateToProcess
    {
        public Guid Id { get; set; }
        public DateOnly ReferenceDate { get; set; }

        public DateToProcess()
        {
            Id = Guid.NewGuid();
        }
    }
}