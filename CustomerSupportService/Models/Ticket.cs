namespace CustomerSupportService.Models
{
    public class Ticket
    {
        public Guid Id { get; set; }
        public Guid? OrderId { get; set; }
        public string? CustomerName { get; set; }
        public string? Message { get; set; }
    }
}
