namespace Dymitro.Models.Domain
{
    public class SportActivity
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
