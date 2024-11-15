﻿using Entities.Models;

namespace ServiceContracts.DTO
{
    public  class BuyOrderResponse
    {
        public Guid BuyOrderID { get; set; }

        public required string StockSymbol { get; set; }

        public required string StockName { get; set; }

        public DateTime DateAndTimeOfOrder { get; set; }

        public int Quantity { get; set; }

        public double Price { get; set; }

        public double TradeAmount { get; set; }

    }

    public static class BuyOrderExtension
    {

        public static BuyOrderResponse ToBuyOrderResponse(this BuyOrder buyOrder)
        {
            return new BuyOrderResponse()
            {
                BuyOrderID = buyOrder.BuyOrderID,
                StockSymbol = buyOrder.StockSymbol,
                StockName = buyOrder.StockName,
                DateAndTimeOfOrder = buyOrder.DateAndTimeOfOrder,
                Quantity = buyOrder.Quantity,
                Price = buyOrder.Price,
                TradeAmount = Math.Round(buyOrder.Price * buyOrder.Quantity, 2)
            };

        }
    }
}
