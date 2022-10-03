using EventlyServer.Data.Dto;
using System;
using System.Collections.Generic;

namespace EventlyServer.Data.Entities
{
    public partial class User : Entity
    {
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string? PhoneNumber { get; set; }
        public string? OtherCommunication { get; set; }
        public bool IsAdmin { get; set; }

        public virtual List<LandingInvitation> LandingInvitations { get; set; }


        public User()
        {
            LandingInvitations = new List<LandingInvitation>();
        }

        public User(UserDto dto)
        {
            Name = dto.Name;
            Email = dto.Email;
            Password = dto.Password;
            PhoneNumber = dto.PhoneNumber;
            OtherCommunication = dto.OtherCommunication;
            IsAdmin = dto.IsAdmin;

            LandingInvitations = new List<LandingInvitation>();
        }
    }
}
