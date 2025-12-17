namespace Client_Management_Database.DTOs
{
    public class IncomeDTO
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        public string Source { get; set; }
        public decimal AmountMonthly { get; set; }
        public DateTime AsOf { get; set; }
        public DateTime AsOfDate { get; set; }
    }
}
