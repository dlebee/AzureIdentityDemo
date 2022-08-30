using CustomerSupportService.Models;

namespace CustomerSupportService.Services
{
    public interface ITicketService
    {
        Task<Guid> CreateTicketAsync(string customerName, string message, Guid? orderId);
        Task<Ticket?> GetTicketAsync(Guid id, CancellationToken cancellationToken = default);
        Task<List<Ticket>> GetTicketsAsync(CancellationToken cancellationToken = default);
    }
}