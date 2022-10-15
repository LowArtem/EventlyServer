using EventlyServer.Data.Entities;
using EventlyServer.Data.Repositories.Abstracts;
using Microsoft.EntityFrameworkCore;

namespace EventlyServer.Data.Repositories
{
    public class LandingInvitationRepository : DbRepository<LandingInvitation>
    {
        public LandingInvitationRepository(ApplicationContext context) : base(context)
        {
        }

        public override IQueryable<LandingInvitation> Items => base.Items.Include(item => item.Responses);
    }
}
