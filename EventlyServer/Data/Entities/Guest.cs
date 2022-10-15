using EventlyServer.Data.Entities.Abstract;
using System;
using System.Collections.Generic;

namespace EventlyServer.Data.Entities
{
    public partial class Guest : Entity
    {
        public Guest()
        {
            Responses = new HashSet<Response>();
        }

        public string Name { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;

        public virtual ICollection<Response> Responses { get; set; }
    }
}
