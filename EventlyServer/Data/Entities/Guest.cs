using System;
using System.Collections.Generic;

namespace EventlyServer.Data.Entities
{
    public partial class Guest : Entity
    {
        public string? Name { get; set; }
        public string? PhoneNumber { get; set; }
        public int? IdLandingInvitation { get; set; }

        public virtual LandingInvitation? IdLandingInvitationNavigation { get; set; }
    }
}
