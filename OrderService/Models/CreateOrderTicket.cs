using System.ComponentModel.DataAnnotations;

namespace OrderService.Models
{
    public class CreateOrderTicket
    {
        [Required]
        public Guid? OrderId { get; set; }
        [Required]
        public string? Message { get; set; }
    }
}
