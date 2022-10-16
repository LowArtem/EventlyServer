using EventlyServer.Data.Entities;
using EventlyServer.Data.Repositories.Abstracts;
using Microsoft.EntityFrameworkCore;

namespace EventlyServer.Data.Repositories
{
    public class TemplateRepository : DbRepository<Template>
    {
        public TemplateRepository(InHolidayContext context) : base(context)
        {
        }

        public override IQueryable<Template> Items => base.Items.Include(item => item.ChosenTypeOfEvent);
    }
}
