using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OrderService.Integration;
using OrderService.Models;
using System.ComponentModel.DataAnnotations;

namespace OrderService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        [HttpPost("reportIssue")]
        public async Task<Guid> CreateTicketForOrderAsync([FromBody] CreateOrderTicket request, [FromServices] ICustomerSupportIntegrationService integrationService) 
        {
            var customerName = "John Doe"; // would normally come from some order service.
            var ticketId = await integrationService.CreateTicketAsync(customerName, request.Message!, request.OrderId);
            return ticketId;
        }
    }
}
