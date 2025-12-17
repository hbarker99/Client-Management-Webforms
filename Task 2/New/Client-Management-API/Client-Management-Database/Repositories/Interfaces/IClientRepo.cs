using Client_Management_Database.DTOs;

namespace Client_Management_Database.Repositories.Interfaces
{
    public interface IClientRepo
    {
        public Task<ClientDTO?> GetClientById(int clientId);
        public Task<SearchResult<ClientInfoDTO>> SearchClients(string? search, ClientStatus? statusLookupValueId, DateOnly? dobFrom, DateOnly? dobTo, int? pageNumber, int? pageSize, string? sortBy, string? sortDir);
    }
}
