using ERP.Core.Helpers;
using ERP.Identity.Domain.DTOs;
using ERP.Identity.Domain.Interfaces;
using ERP.Identity.Infra.Util.Helpers;

namespace ERP.Identity.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly JwtAuthHelper _jwtAuthHelper;

        public UserService(IUserRepository userRepository, JwtAuthHelper jwtAuthHelper)
        {
            _userRepository = userRepository;
            _jwtAuthHelper = jwtAuthHelper;
        }

        public UserAuthenticatedDTO? Authenticate(UserToAuthenticateDTO user)
        {
            try
            {
                var userExists = _userRepository.GetByEmail(user.Email ?? "");

                if (userExists == null)
                    return null;

                if (userExists.Password != user.Password)
                    return null;

                var userAuth = _jwtAuthHelper.Generate(userExists);

                return userAuth;
            }
            catch (Exception ex)
            {
                LogHelper.RiseError(ex);
                throw;
            }
        }
    }
}