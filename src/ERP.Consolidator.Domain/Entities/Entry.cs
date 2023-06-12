using ERP.Core.Entities;

namespace ERP.Consolidator.Domain.Entities
{
    public class Entry : Entity
    {
        public decimal Value { get; set; }
        public DateOnly DateRef { get; set; }
    }
}