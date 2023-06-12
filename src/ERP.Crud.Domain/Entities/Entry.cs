using ERP.Core.Entities;

namespace ERP.Crud.Domain.Entities
{
    public class Entry : Entity
    {
        public string? Description { get; set; }
        public decimal Value { get; set; }
        public DateTime DateOfIssue { get; set; }
    }
}