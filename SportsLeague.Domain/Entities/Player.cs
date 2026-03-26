using SportsLeague.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace SportsLeague.Domain.Entities
{
    public class Player : AuditBase
    {
        public string Firstname { get; set; } = string.Empty;

        public string Lastname { get; set; } = String.Empty;

        public DateTime BirthDate { get; set; }

        public int Number {  get; set; }

        public PlayerPosition Position { get; set; }

        // Foreign key

        public int TeamId { get; set; }

        // Navigation Property

        public Team Team { get; set; } = null!;
    }
}
