using AutoMapper;
using Client_Management_Database.DTOs;
using Client_Management_Database.Models;
using Client_Management_Database.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Client_Management_Database.Repositories
{
    internal class ClientRepo : IClientRepo
    {
        private readonly ILogger<ClientRepo> _logger;
        private readonly IMapper _mapper;
        private readonly IClientManagementDemoContextProcedures _procedures;
        private readonly ClientManagementDemoContext _context;

        public ClientRepo(ILogger<ClientRepo> logger, IMapper mapper, IClientManagementDemoContextProcedures procedures, ClientManagementDemoContext context)
        {
            _logger = logger;
            _mapper = mapper;
            _procedures = procedures;
            _context = context;
        }

        public async Task<ClientDTO?> GetClientById(int clientId)
        {
            var result = await _context.Clients.Where(c => c.ClientId == clientId)
                .Include(c => c.Journals)
                .Include(c => c.Assets)
                .Include(c => c.Liabilities)
                .Include(c => c.Incomes)
                .Include(c => c.Expenditures)
                .FirstOrDefaultAsync();

            return _mapper.Map<ClientDTO>(result);

        }

        public async Task<SearchResult<ClientInfoDTO>> SearchClients(string? search, ClientStatus? statusLookupValueId, DateOnly? dobFrom, DateOnly? dobTo, int? pageNumber, int? pageSize, string? sortBy, string? sortDir)
        {
            var rawData = await _procedures.Client_SearchAsync(search, (int?)statusLookupValueId, dobFrom, dobTo, pageNumber, pageSize, sortBy, sortDir);
            var items = _mapper.Map<List<ClientInfoDTO>>(rawData.Result1);
            var count = rawData.Result2.FirstOrDefault()?.TotalCount ?? 0;
            
            var result = new SearchResult<ClientInfoDTO>
            {
                Results = items,
                Count = count
            };

            return result;
        }
    }
}
