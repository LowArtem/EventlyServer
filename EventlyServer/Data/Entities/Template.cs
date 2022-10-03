using System;
using System.Collections.Generic;

namespace EventlyServer.Data.Entities
{
    public partial class Template : Entity
    {       
        public int Price { get; set; }
        public int? IdTypeOfEvent { get; set; }

        public virtual TypesOfEvent? IdTypeOfEventNavigation { get; set; }
        public virtual List<LandingInvitation> LandingInvitations { get; set; }

        public Template()
        {
            LandingInvitations = new List<LandingInvitation>();
        }
    }
}
