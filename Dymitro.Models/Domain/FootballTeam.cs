namespace Dymitro.Models.Domain
{
    public class FootballTeam
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Country { get; set; }
        public string? Continent { get; set; }
        public short? Active { get; set; }
    }
}
