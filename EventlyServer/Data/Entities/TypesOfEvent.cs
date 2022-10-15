using EventlyServer.Data.Entities.Abstract;
using System;
using System.Collections.Generic;

namespace EventlyServer.Data.Entities
{
    public partial class TypesOfEvent : Entity
    {
        public TypesOfEvent()
        {
            Templates = new HashSet<Template>();
        }

        public string? Name { get; set; }

        public virtual ICollection<Template> Templates { get; set; }
    }
}
