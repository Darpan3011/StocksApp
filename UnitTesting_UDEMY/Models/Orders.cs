using ServiceContracts.DTO;

namespace UnitTesting_UDEMY.Models
{
    public class Orders
    {
        public List<BuyOrderResponse>? BuyOrders { get; set; }

        public List<SellOrderResponse>? SellOrders { get; set; }
    }
}
