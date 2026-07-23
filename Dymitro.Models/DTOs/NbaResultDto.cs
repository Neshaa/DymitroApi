namespace Dymitro.Models.DTOs
{
    public class NbaResultDto
    {
        public int PlayerId { get; set; }
        public string? Season { get; set; }
        public int? Points { get; set; }
        public int? Rebounds { get; set; }
        public int? Asists { get; set; }
    }
}
