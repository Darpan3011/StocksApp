using Entities;
using Entities.Models;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;

namespace ServiceContracts.DTO
{
    public class BuyOrderRequest
    {

        [Required]
        public required string StockSymbol { get; set; }

        [Required]
        public required string StockName { get; set; }

        public DateTime DateAndTimeOfOrder { get; set; } // [Should not be older than Jan 01, 2000]

        [Range(1, 100000, ErrorMessage = "{0} should be between 1 and 100000.")]
        public int Quantity { get; set; }

        [Range(1, 100000, ErrorMessage = "{0} should be between 1 and 100000.")]
        public double Price { get; set; }


        public BuyOrder toBuyOrder()
        {
            return new BuyOrder()
            {
                StockSymbol = StockSymbol,
                StockName = StockName,
                DateAndTimeOfOrder = DateAndTimeOfOrder,
                Quantity = Quantity,
                Price = Price,
            };
        }

        public override string ToString()
        {
            return $"StockSymbol: {StockSymbol}, StockName: {StockName}, DateAndTimeOfOrder: {DateAndTimeOfOrder}, Quantity: {Quantity}, Price: {Price}";
        }
    }

    public static class StockTradeExtensions
    {
        public static BuyOrderRequest ToBuyOrderRequest(this StockTrade stockTrade)
        {
            return new BuyOrderRequest
            {
                StockSymbol = stockTrade.StockSymbol,
                StockName = stockTrade.StockName,
                Quantity = stockTrade.Quantity,
                Price = stockTrade.Price,
                DateAndTimeOfOrder = DateTime.Now
            };
        }
    }
}
