using System.ComponentModel.DataAnnotations;

namespace OrderService.Integration
{
    public class CustomerSupportIntegrationService : ICustomerSupportIntegrationService
    {
        private readonly IHttpClientFactory httpClientFactory;

        public CustomerSupportIntegrationService(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory;
        }

        public async Task<Guid> CreateTicketAsync(string customerName, string message, Guid? orderId)
        {
            var client = httpClientFactory.CreateClient("CustomerSupport");
            var response = await client.PostAsJsonAsync("api/customerSupport", new CreateTicketRequest()
            {
                OrderId = orderId,
                CustomerName = customerName,
                Message = message
            });
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<Guid>();
        }
    }
}
