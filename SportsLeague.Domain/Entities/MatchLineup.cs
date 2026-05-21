namespace SportsLeague.Domain.Entities
{
    public class MatchLineup : AuditBase
    {
        public int Id { get; set; }

        public int MatchId { get; set; }

        public int PlayerId { get; set; }

        public bool IsStarter { get; set; }

        public string Position { get; set; } = string.Empty;

        //Navigation Property

        public Player Player { get; set; } = null!;

        public Match Match { get; set; } = null!;
    }
}
