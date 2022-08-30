namespace OrderService.Integration
{
    public interface ICustomerSupportIntegrationService
    {
        Task<Guid> CreateTicketAsync(string customerName, string message, Guid? orderId);
    }
}