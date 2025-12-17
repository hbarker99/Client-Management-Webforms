namespace Client_Management_Database.DTOs
{
    public class JournalDTO
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        public JournalType Type { get; set; }
        public DateTime OccurredAt { get; set; }
        public string Body { get; set; }
        public string Author { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
