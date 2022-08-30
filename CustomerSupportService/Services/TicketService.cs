using CustomerSupportService.Models;
using System.Collections.Concurrent;

namespace CustomerSupportService.Services
{
    public class TicketService : ITicketService
    {
        private ConcurrentDictionary<Guid, Ticket> state = new ConcurrentDictionary<Guid, Ticket>();

        public Task<List<Ticket>> GetTicketsAsync(CancellationToken cancellationToken = default)
        {
            return Task.FromResult(state.Values.ToList());
        }

        public Task<Ticket?> GetTicketAsync(Guid id, CancellationToken cancellationToken = default)
        {
            Ticket? result = state.GetValueOrDefault(id);
            return Task.FromResult(result);
        }

        public Task<Guid> CreateTicketAsync(string customerName, string message, Guid? orderId)
        {
            var newTicket = new Ticket();
            newTicket.Id = Guid.NewGuid();
            newTicket.Message = message;
            newTicket.CustomerName = customerName;
            newTicket.OrderId = orderId;
            state.TryAdd(newTicket.Id, newTicket);
            return Task.FromResult(newTicket.Id);
        }
    }
}
