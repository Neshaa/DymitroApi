namespace Dymitro.Models.DTOs
{
    public class SportCompetitionStatsDto
    {
        public SportCountryDto? Name { get; set; }
        public int FirstCount { get; set; }
        public int SecondCount { get; set; }
        public int ThirdCount { get; set; }
        public int Total { get; set; }
    }
}
