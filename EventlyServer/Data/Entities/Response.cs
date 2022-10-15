using System;
using System.Collections.Generic;

namespace EventlyServer.Data.Entities
{
    public partial class Response
    {
        public DateOnly Date { get; set; }
        public int IdGuest { get; set; }
        public int IdLandingInvitation { get; set; }

        public virtual Guest IdGuestNavigation { get; set; } = null!;
        public virtual LandingInvitation IdLandingInvitationNavigation { get; set; } = null!;
    }
}
