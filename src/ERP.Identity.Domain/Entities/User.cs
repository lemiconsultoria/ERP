using ERP.Core.Entities;

namespace ERP.Identity.Domain.Entities
{
    public class User : Entity
    {
        public string? Role { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
    }
}