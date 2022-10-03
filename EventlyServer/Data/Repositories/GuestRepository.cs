using EventlyServer.Data.Entities;
using EventlyServer.Data.Repositories.Abstracts;

namespace EventlyServer.Data.Repositories
{
    public class GuestRepository : DbRepository<Guest>
    {
        public GuestRepository(ApplicationContext context) : base(context)
        {
        }

        public override IQueryable<Guest> Items => base.Items;
    }
}
