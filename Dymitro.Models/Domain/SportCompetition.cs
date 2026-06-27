namespace Dymitro.Models.Domain
{
    public class SportCompetition
    {
        public int Id { get; set; }
        public string? Sport { get; set; }
        public int? Year { get; set; }
        public string? Host { get; set; }
        public string? First { get; set; }
        public string? Second { get; set; }
        public string? Third { get; set; }
        public string? Competition { get; set; }
    }
}
