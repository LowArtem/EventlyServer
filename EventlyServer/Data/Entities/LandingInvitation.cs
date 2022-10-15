using EventlyServer.Data.Entities.Abstract;

namespace EventlyServer.Data.Entities
{
    public partial class LandingInvitation : Entity
    {
        public string Link { get; set; } = null!;
        public DateOnly StartDate { get; set; }
        public DateOnly FinishDate { get; set; }
        public int? IdClient { get; set; }
        public int? IdTemplate { get; set; }

        public virtual User? Client { get; set; }
        public virtual Template? ChosenTemplate { get; set; }
        public virtual List<Response> Responses { get; set; }

        public LandingInvitation()
        {
            Responses = new List<Response>();
        }

    }
}