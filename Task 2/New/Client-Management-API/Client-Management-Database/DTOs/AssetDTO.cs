namespace Client_Management_Database.DTOs
{
    public class AssetDTO
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        public string Type { get; set; }
        public decimal Value { get; set; }
        public DateTime AsOf { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Provider { get; set; }
    }
}
