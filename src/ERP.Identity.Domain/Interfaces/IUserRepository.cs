using ERP.Identity.Domain.Entities;

namespace ERP.Identity.Domain.Interfaces
{
    public interface IUserRepository
    {
        public User? GetByEmail(string email);
    }
}