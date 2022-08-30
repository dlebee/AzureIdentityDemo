namespace OrderService.Integration
{
    internal class CreateTicketRequest
    {
        public Guid? OrderId { get; set; }
        public string? Message { get; set; }
        public string? CustomerName { get; set; }
    }
}
