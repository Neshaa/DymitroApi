namespace Dymitro.Models.DTOs
{
    public class NbaPlayerDto
    {
        public int Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Country { get; set; }
        public short? Active { get; set; }
        public short? Balkan { get; set; }
    }
}
