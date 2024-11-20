using Entities.Models;
using Microsoft.EntityFrameworkCore;
using ServiceContracts;
using ServiceContracts.DTO;
using Entities;
using Exceptions;

namespace Services
{
    public class StocksService : IStocksService
    {
        private readonly OrdersDbContext _dbContext;

        public StocksService(OrdersDbContext dbContext) { 
            _dbContext = dbContext;
        }

        public async Task<BuyOrderResponse> CreateBuyOrder(BuyOrderRequest? buyOrderRequest)
        {
            if (buyOrderRequest == null)
            {
                throw new ArgumentNullException(nameof(buyOrderRequest));
            }

            if (string.IsNullOrWhiteSpace(buyOrderRequest.StockSymbol))
            {
                throw new ArguementExceptionError("Stock symbol cannot be null or empty.", nameof(buyOrderRequest.StockSymbol));
            }

            if (buyOrderRequest.Quantity < 1 || buyOrderRequest.Quantity > 100000)
            {
                throw new ArguementExceptionError("Quantity should be between 1 and 100000.", nameof(buyOrderRequest.Quantity));
            }

            if (buyOrderRequest.Price < 1 || buyOrderRequest.Price > 100000)
            {
                throw new ArguementExceptionError("Price should be between 1 and 100000.", nameof(buyOrderRequest.Price));
            }

            if (buyOrderRequest.DateAndTimeOfOrder < new DateTime(2000, 1, 1))
            {
                throw new ArguementExceptionError("Date and Time of Order should be on or after 2000-01-01.", nameof(buyOrderRequest.DateAndTimeOfOrder));
            }

            BuyOrder buyOrder = buyOrderRequest.toBuyOrder();
            buyOrder.BuyOrderID = Guid.NewGuid();
            _dbContext.sp_AddBuyOrder(buyOrder);

            BuyOrderResponse buyOrderResponse = buyOrder.ToBuyOrderResponse();

            return await Task.FromResult(buyOrderResponse);
        }

        public async Task<SellOrderResponse> CreateSellOrder(SellOrderRequest? sellOrderRequest)
        {
            if (sellOrderRequest == null)
            {
                throw new ArgumentNullException(nameof(sellOrderRequest));
            }

            if (string.IsNullOrWhiteSpace(sellOrderRequest.StockSymbol))
            {
                throw new ArgumentException("Stock symbol cannot be null or empty.", nameof(sellOrderRequest.StockSymbol));
            }

            if (sellOrderRequest.Quantity < 1 || sellOrderRequest.Quantity > 100000)
            {
                throw new ArgumentException("Quantity should be between 1 and 100000.", nameof(sellOrderRequest.Quantity));
            }

            if (sellOrderRequest.Price < 1 || sellOrderRequest.Price > 10000)
            {
                throw new ArgumentException("Price should be between 1 and 10000.", nameof(sellOrderRequest.Price));
            }

            if (sellOrderRequest.DateAndTimeOfOrder < new DateTime(2000, 1, 1))
            {
                throw new ArgumentException("Date and Time of Order should be on or after 2000-01-01.", nameof(sellOrderRequest.DateAndTimeOfOrder));
            }

            SellOrder sellOrder = sellOrderRequest.toSellOrder();
            sellOrder.SellOrderID = Guid.NewGuid();
            _dbContext.sp_AddSellOrder(sellOrder);

            SellOrderResponse sellOrderResponse = sellOrder.ToSellOrderResponse();
            return await Task.FromResult(sellOrderResponse);
        }

        public async Task<List<BuyOrderResponse>> GetBuyOrders()
        {
            return await _dbContext.buyOrders.OrderByDescending(temp=>temp.DateAndTimeOfOrder).Select(temp => temp.ToBuyOrderResponse()).ToListAsync();
        }

        public async Task<List<SellOrderResponse>> GetSellOrders()
        {
            return await _dbContext.sellOrders.OrderByDescending(temp => temp.DateAndTimeOfOrder).Select(temp => temp.ToSellOrderResponse()).ToListAsync();
        }
    }
}
