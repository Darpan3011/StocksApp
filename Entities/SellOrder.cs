using System.ComponentModel.DataAnnotations;

namespace Entities.Models
{
    public class SellOrder
    {

        public Guid SellOrderID { get; set; }

        [Required]
        public required string StockSymbol { get; set; }

        [Required]
        public required string StockName { get; set; }

        public DateTime DateAndTimeOfOrder { get; set; }

        [Range(1, 100000, ErrorMessage = "Value should be between 1 and 100000.")]
        public int Quantity { get; set; }

        [Range(1, 100000, ErrorMessage = "Value should be between 1 and 100000.")]
        public double Price { get; set; }


    }
}
