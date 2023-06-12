using ERP.Identity.Domain.DTOs;

namespace ERP.Identity.Domain.Interfaces
{
    public interface IUserService
    {
        UserAuthenticatedDTO? Authenticate(UserToAuthenticateDTO user);
    }
}