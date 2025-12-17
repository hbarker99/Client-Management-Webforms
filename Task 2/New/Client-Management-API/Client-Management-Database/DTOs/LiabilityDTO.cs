namespace Client_Management_Database.DTOs
{
    public class LiabilityDTO
    {
        public int Id { get; set; }
        public int ClientId { get; set; }

        public string Type { get; set; }

        public decimal Balance { get; set; }

        public decimal? Rate { get; set; }

        public DateTime AsOf { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
