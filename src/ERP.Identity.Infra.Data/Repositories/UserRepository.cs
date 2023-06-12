using ERP.Identity.Domain.Entities;
using ERP.Identity.Domain.Interfaces;
using ERP.Identity.Infra.Data.Context;

namespace ERP.Identity.Infra.Data.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly FakeContext _context;

        public UserRepository(FakeContext context)
        {
            _context = context;
        }

        public User? GetByEmail(string email)
        {
            if (string.IsNullOrEmpty(email))
                throw new ArgumentNullException("email is required");

            var user = _context.Users.Where(x => x.Email?.ToLower() == email.ToLower()).FirstOrDefault();

            return user;
        }
    }
}