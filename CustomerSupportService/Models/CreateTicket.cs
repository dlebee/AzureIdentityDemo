using System.ComponentModel.DataAnnotations;

namespace CustomerSupportService.Models
{
    public class CreateTicket
    {
        public Guid? OrderId { get; set; }
        [Required]
        public string? Message { get; set; }
        [Required]
        public string? CustomerName { get; set; }
    }
}
