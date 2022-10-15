using EventlyServer.Data.Entities.Abstract;
using System;
using System.Collections.Generic;

namespace EventlyServer.Data.Entities
{
    public partial class User : Entity
    {
        public User()
        {
            LandingInvitations = new HashSet<LandingInvitation>();
        }

        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string? PhoneNumber { get; set; }
        public string? OtherCommunication { get; set; }
        public bool IsAdmin { get; set; }

        public virtual ICollection<LandingInvitation> LandingInvitations { get; set; }
    }
}
