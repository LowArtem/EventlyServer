using EventlyServer.Data.Entities.Abstract;

namespace EventlyServer.Data.Entities;

public partial class TypesOfEvent : Entity
{
    public string? Name { get; set; }

    public virtual List<Template> Templates { get; set; }

    public TypesOfEvent()
    {
        Templates = new List<Template>();
    }

    public TypesOfEvent(string? name)
    {
        Name = name;
        Templates = new List<Template>();
    }
}