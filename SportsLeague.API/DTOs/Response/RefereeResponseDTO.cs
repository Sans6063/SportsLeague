namespace SportsLeague.API.DTOs.Response
{
    public class RefereeResponseDTO
    {
        public int Id { get; set; }

        public string Firstname { get; set; } = string.Empty;

        public string Lastname { get; set; } = string.Empty;

        public string Nationality {  get; set; } = string.Empty;

        public DateTime CreateAt { get; set; }

        public DateTime? UpdateAt { get; set; }
    }
}
