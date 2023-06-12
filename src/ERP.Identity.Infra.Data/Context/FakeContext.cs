using ERP.Identity.Domain.Entities;

namespace ERP.Identity.Infra.Data.Context
{
    public class FakeContext
    {
        public readonly IList<User> Users;

        public FakeContext()
        {
            Users = new List<User>
            {
                new User
                {
                    Role = "ADMIN",
                    Email = "admin@erp.com.br",
                    Name = "Admin",
                    Password = "admin"
                },

                new User
                {
                    Role = "REPORT",
                    Email = "report@erp.com.br",
                    Name = "Report",
                    Password = "report"
                },
            };
        }
    }
}