using Client_Management_API.Services.Interfaces;
using Client_Management_Database.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace Client_Management_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClientsController : ControllerBase
    {
        private readonly ILogger<ClientsController> _logger;
        private readonly IClientService _clientService;

        public ClientsController(ILogger<ClientsController> logger, IClientService clientService)
        {
            _logger = logger;
            _clientService = clientService;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ClientDTO>> GetClientById(int id)
        {
            var result = await _clientService.GetClientById(id);

            if (result is null)
                return NotFound();

            return Ok(result);
        }

        [HttpGet("")]
        public async Task<ActionResult<SearchResult<ClientInfoDTO>>> SearchClients(string? search, ClientStatus? status, DateOnly? dobFrom, DateOnly? dobTo, int? pageNumber, int? pageSize, string? sortBy, string? sortDir)
        {
            var result = await _clientService.SearchClients(search, status, dobFrom, dobTo, pageNumber, pageSize, sortBy, sortDir);
            return Ok(result);
        }
    }
}
