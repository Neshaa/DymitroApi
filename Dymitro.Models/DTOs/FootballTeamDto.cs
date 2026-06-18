namespace Dymitro.Models.DTOs
{
    public class FootballTeamDto
    {
        public int? Id { get; set; }
        public string? Name { get; set; }
        public string? Country { get; set; }
        public string? Continent { get; set; }
        public short? Active { get; set; }
        public string TeamFormated
        {
            get
            {
                return String.Concat(Country, " - ", Name);
            }
        }
    }
}
