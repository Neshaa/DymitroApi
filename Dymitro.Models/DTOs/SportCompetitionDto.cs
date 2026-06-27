namespace Dymitro.Models.DTOs
{
    public class SportCompetitionDto
    {
        public string? Sport { get; set; }
        public int? Year { get; set; }
        public string? Host { get; set; }
        public SportCountryDto? First { get; set; }
        public SportCountryDto? Second { get; set; }
        public SportCountryDto? Third { get; set; }
        public string? Competition { get; set; }
    }
}
