namespace UnitTesting_UDEMY.Models
{
    public class PDFModel
    {
        public DateTime DateAndTimeOfOrder { get; set; }
        public string StockName { get; set; }
        public string StockSymbol { get; set; }
        public string OrderType { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }
        public double TradeAmount { get; set; }

    }

}
