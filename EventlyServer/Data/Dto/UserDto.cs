namespace EventlyServer.Data.Dto
{
    public class UserDto
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string? PhoneNumber { get; set; }
        public string? OtherCommunication { get; set; }
        public bool IsAdmin { get; set; }

        public UserDto(string name, string email, string password, string? phoneNumber, string? otherCommunication = null, bool isAdmin = false)
        {
            Name = name;
            Email = email;
            Password = password;
            PhoneNumber = phoneNumber;
            OtherCommunication = otherCommunication;
            IsAdmin = isAdmin;
        }
    }
}
