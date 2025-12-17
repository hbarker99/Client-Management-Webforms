namespace Client_Management_Database.DTOs
{
    public enum ClientStatus
    {
        UNKNOWN = 0,
        NEW = 1,
        QUOTES_SENT = 2,
        IFA_CALL_BOOKED = 3,
        APP_PACK_SENT = 4,
        APP_PACK_BACK = 5,
        COMPLETED = 6,
        NOT_PROCEEDING = 7
    }

    public enum JournalType
    {
        CALL = 8,
        EMAIL = 9,
        NOTE = 10,
        SYSTEM = 11
    }
}
