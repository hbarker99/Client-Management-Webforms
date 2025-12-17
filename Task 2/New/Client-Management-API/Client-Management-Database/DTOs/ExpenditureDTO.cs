namespace Client_Management_Database.DTOs
{
    public class ExpenditureDTO
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        public string Category { get; set; }
        public decimal AmountMonthly { get; set; }
        public DateTime AsOf { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
