namespace SportsLeague.Domain.Entities
{
    public abstract class AuditBase
    {
        public int Id { get; set; }

        public DateTime CreateAt { get; set; } = DateTime.UtcNow;

        public DateTime? UpdateAt { get; set; }
    }
}
