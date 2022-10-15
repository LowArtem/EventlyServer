using EventlyServer.Data.Entities;
using EventlyServer.Data.Repositories.Abstracts;
using Microsoft.EntityFrameworkCore;

namespace EventlyServer.Data.Repositories
{
    public class UserRepository : DbRepository<User>
    {
        public UserRepository(ApplicationContext context) : base(context)
        {
        }

        public override IQueryable<User> Items => base.Items.Include(items => items.LandingInvitations);
    }
}
