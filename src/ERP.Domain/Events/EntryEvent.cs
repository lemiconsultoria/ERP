using ERP.Core.Messages;

namespace ERP.Domain.Events
{
    public abstract class BaseEntryEvent : Message
    {
        public long Id { get; set; }
        public decimal Value { get; set; }
        public DateTime DateOfIssue { get; set; }
        public DateTime Created { get; set; }
        public DateTime? Updated { get; set; }
    }

    public class DebitEntryEvent : BaseEntryEvent
    { }

    public class CreditEntryEvent : BaseEntryEvent
    { }
}