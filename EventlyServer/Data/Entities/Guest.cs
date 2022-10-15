using EventlyServer.Data.Entities.Abstract;

namespace EventlyServer.Data.Entities
{
    public partial class Guest : Entity
    {
        public string Name { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;

        public virtual List<Response> Responses { get; set; }

        public Guest()
        {
            Responses = new List<Response>();
        }
    }
}
