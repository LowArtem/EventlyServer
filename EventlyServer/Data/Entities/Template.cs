using EventlyServer.Data.Entities.Abstract;

namespace EventlyServer.Data.Entities;

/// <summary>
/// Сущность шаблона приглашения
/// </summary>
public partial class Template : Entity
{
    public int Price { get; set; }
    public string Name { get; set; }
    public int IdTypeOfEvent { get; set; }
    public string FilePath { get; set; } = null!;
    public string PreviewPath { get; set; } = null!;

    public virtual TypesOfEvent ChosenTypeOfEvent { get; set; } = null!;
    public virtual List<LandingInvitation> LandingInvitations { get; set; }

    public Template()
    {
        LandingInvitations = new List<LandingInvitation>();
    }

    public Template(int price, string name, int idTypeOfEvent, string filePath, string previewPath)
    {
        Price = price;
        Name = name;
        IdTypeOfEvent = idTypeOfEvent;
        FilePath = filePath;
        PreviewPath = previewPath;
        LandingInvitations = new List<LandingInvitation>();
    }
    
    public Template(int id, int price, string name, int idTypeOfEvent, string filePath, string previewPath)
    {
        Id = id;
        Price = price;
        Name = name;
        IdTypeOfEvent = idTypeOfEvent;
        FilePath = filePath;
        PreviewPath = previewPath;
        LandingInvitations = new List<LandingInvitation>();
    }
}