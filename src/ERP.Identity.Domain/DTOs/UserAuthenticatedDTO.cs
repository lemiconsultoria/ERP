using ERP.Core.DTOs;

namespace ERP.Identity.Domain.DTOs
{
    public class UserAuthenticatedDTO : DTOBase
    {
        public string? Email { get; set; }
        public string? Token { get; set; }
        public DateTime ExpireAt { get; set; }
    }
}