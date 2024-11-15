using System.ComponentModel.DataAnnotations;
using Entities;
using Entities.Models;

namespace ServiceContracts.DTO
{
    public class SellOrderRequest
    {
        [Required]
        public required string StockSymbol { get; set; }

        [Required]
        public required string StockName { get; set; }

        public DateTime DateAndTimeOfOrder { get; set; } // [Should not be older than Jan 01, 2000]

        [Range(1, 100000, ErrorMessage = "Value should be between 1 and 100000.")]
        public int Quantity { get; set; }

        [Range(1, 100000, ErrorMessage = "Value should be between 1 and 100000.")]
        public double Price { get; set; }

        public SellOrder toSellOrder()
        {
            return new SellOrder()
            {
                StockSymbol = StockSymbol,
                StockName = StockName,
                DateAndTimeOfOrder = DateAndTimeOfOrder,
                Quantity = Quantity,
                Price = Price,
            };
        }
    }
}
