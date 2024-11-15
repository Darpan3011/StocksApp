using Entities.Models;
using ServiceContracts.DTO;

namespace ServiceContracts
{
    public interface IStocksService
    {
        public Task<BuyOrderResponse> CreateBuyOrder(BuyOrderRequest? buyOrderRequest);
        public Task<SellOrderResponse> CreateSellOrder(SellOrderRequest? sellOrderRequest);

        public Task<List<BuyOrderResponse>> GetBuyOrders();

        public Task<List<SellOrderResponse>> GetSellOrders();
    }
}
