using Client_Management_API.Services.Interfaces;
using Client_Management_Database.DTOs;
using Client_Management_Database.Repositories.Interfaces;

namespace Client_Management_API.Services
{
    public class ClientService : IClientService
    {
        private readonly ILogger<ClientService> _logger;
        private readonly IClientRepo _clientRepo;

        public ClientService(ILogger<ClientService> logger, IClientRepo clientRepo)
        {
            _logger = logger;
            _clientRepo = clientRepo;
        }

        public async Task<ClientDTO?> GetClientById(int clientId)
        {
            return await _clientRepo.GetClientById(clientId);
        }

        public async Task<SearchResult<ClientInfoDTO>> SearchClients(string? search, ClientStatus? statusLookupValueId, DateOnly? dobFrom, DateOnly? dobTo, int? pageNumber, int? pageSize, string? sortBy, string? sortDir)
        {
            if (sortBy == "name")
                sortBy = "lastName";


            return await _clientRepo.SearchClients(search, statusLookupValueId, dobFrom, dobTo, pageNumber, pageSize, sortBy, sortDir);
        }
    }
}
