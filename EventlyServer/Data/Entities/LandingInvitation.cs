using System;
using System.Collections.Generic;

namespace EventlyServer.Data.Entities
{
    public partial class LandingInvitation : Entity
    {     
        public string Link { get; set; } = null!;
        public DateTime StartDate { get; set; }
        public DateTime FinishDate { get; set; }
        public int? IdClient { get; set; }
        public int? IdTemplate { get; set; }

        public virtual User? IdClientNavigation { get; set; }
        public virtual Template? IdTemplateNavigation { get; set; }
        public virtual List<Guest> Guests { get; set; }

        public LandingInvitation()
        {
            Guests = new List<Guest>();
        }
    }
}
