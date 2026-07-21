namespace Dymitro.Models.Domain
{
    public class NbaPlayer
    {
        public int Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Country { get; set; }
        public bool? Active { get; set; }
        public bool? Balkan { get; set; }
    }
}
