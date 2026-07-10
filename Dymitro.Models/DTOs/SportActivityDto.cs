namespace Dymitro.Models.DTOs
{
    public class SportActivityDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public DateTime? Ddate { get; set; }
        public decimal? Duration { get; set; }
        public decimal? Distance { get; set; }
        public int? ElevationGain { get; set; }
        public int? ElevationLoss { get; set; }
        public decimal? AvgSpeed { get; set; }
        public decimal? MaxSpeed { get; set; }
        public decimal? MovingTime { get; set; }
    }
}
