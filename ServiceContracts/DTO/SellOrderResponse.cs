using System.ComponentModel.DataAnnotations;
using Entities;
using Entities.Models;

namespace ServiceContracts.DTO
{
    public class SellOrderResponse
    {
        public Guid SellOrderID { get; set; }

        public required string StockSymbol { get; set; }

        public required string StockName { get; set; }

        public DateTime DateAndTimeOfOrder { get; set; }

        [Range(1, 100000, ErrorMessage = "{0} should be between 1 and 100000.")]
        public int Quantity { get; set; }

        [Range(1, 100000, ErrorMessage = "{0} should be between 1 and 100000.")]

        public double Price { get; set; }

        public double TradeAmount { get; set; }
    }

    public static class SellOrderExtension
    {
        public static SellOrderResponse ToSellOrderResponse(this SellOrder sellOrder)
        {
            return new SellOrderResponse()
            {
                SellOrderID = sellOrder.SellOrderID,
                StockSymbol = sellOrder.StockSymbol,
                StockName = sellOrder.StockName,
                DateAndTimeOfOrder = sellOrder.DateAndTimeOfOrder,
                Quantity = sellOrder.Quantity,
                Price = sellOrder.Price,
                TradeAmount = Math.Round(sellOrder.Price * sellOrder.Quantity, 2)
            };

        }
        public static SellOrderRequest ToSellOrderRequest(this StockTrade stockTrade)
        {
            return new SellOrderRequest
            {
                StockSymbol = stockTrade.StockSymbol,
                StockName = stockTrade.StockName,
                Quantity = stockTrade.Quantity,
                Price = stockTrade.Price
            };
        }
    }
}
