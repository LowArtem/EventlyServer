using EventlyServer.Data.Entities.Abstract;
using System;
using System.Collections.Generic;

namespace EventlyServer.Data.Entities
{
    public partial class LandingInvitation : Entity
    {
        public LandingInvitation()
        {
            Responses = new HashSet<Response>();
        }

        public string Link { get; set; } = null!;
        public DateOnly StartDate { get; set; }
        public DateOnly FinishDate { get; set; }
        public int? IdClient { get; set; }
        public int? IdTemplate { get; set; }

        public virtual User? IdClientNavigation { get; set; }
        public virtual Template? IdTemplateNavigation { get; set; }
        public virtual ICollection<Response> Responses { get; set; }
    }
}
