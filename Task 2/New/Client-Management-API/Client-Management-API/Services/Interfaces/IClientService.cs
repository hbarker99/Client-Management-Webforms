using Client_Management_Database.DTOs;
using Client_Management_Database.Models;

namespace Client_Management_API.Services.Interfaces
{
    public interface IClientService
    {
        public Task<ClientDTO?> GetClientById(int clientId);
        public Task<SearchResult<ClientInfoDTO>> SearchClients(string? search, ClientStatus? statusLookupValueId, DateOnly? dobFrom, DateOnly? dobTo, int? pageNumber, int? pageSize, string? sortBy, string? sortDir);
    }
}
