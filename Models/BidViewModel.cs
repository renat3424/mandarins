using System.ComponentModel.DataAnnotations;

namespace mandarinProject1.Models
{
    public class BidViewModel
    {
        public int Id { get; set; }
        [Required]
        public int CurrentPrize { get; set; }
    }
}
