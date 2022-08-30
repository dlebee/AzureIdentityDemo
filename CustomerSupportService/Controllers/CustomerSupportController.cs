using CustomerSupportService.Models;
using CustomerSupportService.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CustomerSupportService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize("CustomerSupport")]
    public class CustomerSupportController
    {
        [HttpGet]
        public Task<List<Ticket>> Get([FromServices] ITicketService ticketService, CancellationToken cancellationToken) 
            => ticketService.GetTicketsAsync(cancellationToken);

        [HttpGet("{id}")]
        public Task<Ticket?> Get([FromServices] ITicketService ticketService, Guid id, CancellationToken cancellationToken) 
            => ticketService.GetTicketAsync(id, cancellationToken);

        [HttpPost]
        public Task<Guid> CreateTicketAsync([FromBody] CreateTicket request, [FromServices] ITicketService ticketService) 
            => ticketService.CreateTicketAsync(request.CustomerName!, request.Message!, request.OrderId);
    }
}
