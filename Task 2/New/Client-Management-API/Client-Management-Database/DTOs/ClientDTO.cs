namespace Client_Management_Database.DTOs
{
    public class ClientInfoDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public DateOnly DateOfBirth { get; set; }
        public ClientStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class ClientDTO : ClientInfoDTO
    {
        public List<AssetDTO> Assets { get; set; } = new List<AssetDTO>();
        public List<IncomeDTO> Incomes { get; set; } = new List<IncomeDTO>();
        public List<ExpenditureDTO> Expenditures { get; set; } = new List<ExpenditureDTO>();
        public List<LiabilityDTO> Liabilities { get; set; } = new List<LiabilityDTO>();
        public List<JournalDTO> Journals { get; set; } = new List<JournalDTO>();
    }
}
