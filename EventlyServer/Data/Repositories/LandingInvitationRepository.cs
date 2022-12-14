using EventlyServer.Data.Entities;
using EventlyServer.Data.Repositories.Abstracts;
using Microsoft.EntityFrameworkCore;

namespace EventlyServer.Data.Repositories;

public class LandingInvitationRepository : DbRepository<LandingInvitation>
{
    public LandingInvitationRepository(InHolidayContext context) : base(context)
    {
    }

    public override IQueryable<LandingInvitation> Items => base.Items
        .Include(item => item.Guests)
        .Include(item => item.ChosenTemplate.ChosenTypeOfEvent);
}