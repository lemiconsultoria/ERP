using ERP.Core.DTOs;

namespace ERP.Identity.Domain.DTOs
{
    public class UserToAuthenticateDTO : DTOBase
    {
        public string? Email { get; set; }
        public string? Password { get; set; }
    }
}