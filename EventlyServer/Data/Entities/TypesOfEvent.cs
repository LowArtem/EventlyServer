using EventlyServer.Data.Entities.Abstract;

namespace EventlyServer.Data.Entities;

/// <summary>
/// Сущность типа приглашения (свадьба, корпоратив, день рождения ...)
/// </summary>
public partial class TypesOfEvent : Entity
{
    public string Name { get; set; } = null!;

    public virtual List<Template> Templates { get; set; }

    public TypesOfEvent()
    {
        Templates = new List<Template>();
    }

    public TypesOfEvent(string name)
    {
        Name = name;
        Templates = new List<Template>();
    }
}