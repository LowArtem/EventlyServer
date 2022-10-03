using EventlyServer.Data.Entities;
using EventlyServer.Data.Repositories.Abstracts;
using Microsoft.EntityFrameworkCore;

namespace EventlyServer.Data.Repositories
{
    public class TypesOfEventRepository : DbRepository<TypesOfEvent>
    {
        public TypesOfEventRepository(ApplicationContext context) : base(context)
        {
        }

        public override IQueryable<TypesOfEvent> Items => base.Items;
    }
}
