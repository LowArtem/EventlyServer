using EventlyServer.Data.Entities.Abstract;
using System;
using System.Collections.Generic;

namespace EventlyServer.Data.Entities
{
    public partial class Template : Entity
    {
        public Template()
        {
            LandingInvitations = new HashSet<LandingInvitation>();
        }

        public decimal Price { get; set; }
        public int? IdTypeOfEvent { get; set; }

        public virtual TypesOfEvent? IdTypeOfEventNavigation { get; set; }
        public virtual ICollection<LandingInvitation> LandingInvitations { get; set; }
    }
}
