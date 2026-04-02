using SportsLeague.Domain.Enums;
using System.Runtime.CompilerServices;

namespace SportsLeague.Domain.Entities
{
    public class Sponsor : AuditBase
    {
        public string Name { get; set; } = string.Empty;

        public string Contactemail { get; set; } = string.Empty;

        public string? Phone { get; set; }

        public string? Websiteurl { get; set; }

        public SponsorCategory Category { get; set; }

       //Relación N:M
       public ICollection<TournamentSponsor> TournamentSponsors { get; set; }



    }
}
