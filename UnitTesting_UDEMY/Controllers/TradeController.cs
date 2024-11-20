using Entities;
using Microsoft.AspNetCore.Mvc;
using Rotativa.AspNetCore;
using ServiceContracts;
using ServiceContracts.DTO;
using UnitTesting_UDEMY.Filters;
using UnitTesting_UDEMY.Models;

namespace UnitTesting_UDEMY.Controllers
{
    [Route("Trade")]
    public class TradeController : Controller
    {
        public readonly IConfiguration _configuration;
        private readonly IFinnhubService _finnhubService;
        private readonly IStocksService _stocksService;
        private readonly ILogger<TradeController> _logger;

        public TradeController(IFinnhubService finnhubService, IConfiguration configuration, IStocksService stocksService, ILogger<TradeController> logger)
        {
            _configuration = configuration;
            _finnhubService = finnhubService;
            _stocksService = stocksService;
            _logger = logger;
        }

        [Route("/")]
        [Route("~/Trade/Index/{symbol?}")]
        public async Task<ActionResult> Index(string? symbol)
        {
            _logger.LogInformation("Hey in Index action");
            string stockSymbol = symbol ?? _configuration.GetValue("TradingOptions:DefaultStockSymbol", "MSFT")!;
            ViewBag.Symbol = stockSymbol;

            Dictionary<string, object>? companyProfile = await _finnhubService.GetCompanyProfile(stockSymbol);
            Dictionary<string, object>? stockPriceQuote = await _finnhubService.GetStockPriceQuote(stockSymbol);

            if (companyProfile == null || stockPriceQuote == null || !companyProfile.ContainsKey("name") || !stockPriceQuote.ContainsKey("c"))
            {
                ViewBag.ErrorMessage = "Invalid stock symbol. Please try another.";
                return View(new StockTrade() { StockName = "", StockSymbol = "" });
            }

            StockTrade stockTrade = new StockTrade()
            {
                StockName = companyProfile["name"].ToString()!,
                StockSymbol = companyProfile["ticker"].ToString()!,
                Price = Convert.ToDouble(stockPriceQuote["c"].ToString()!),
                Quantity = 0
            };

            _logger.LogInformation("Out from Index action");
            return View(stockTrade);
        }

        [HttpPost]
        [Route("BuyOrder")]
        [TypeFilter(typeof(ActionFilterForModelValidation))]
        public async Task<IActionResult> BuyOrder(StockTrade stockTrade)
        {
            _logger.LogInformation($"Buy Order Request {stockTrade}");
            //if (!ModelState.IsValid)
            //{
            //    foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
            //    {
            //        Console.WriteLine(error.ErrorMessage);
            //    }

            //    ViewBag.ErrorMessage = "There was an error with your input. Please correct it and try again.";
            //    return View("Index", stockTrade);
            //}
            BuyOrderRequest buyOrderRequest = stockTrade.ToBuyOrderRequest();
            await _stocksService.CreateBuyOrder(buyOrderRequest);
            _logger.LogInformation($"Ending Buy Order Request {stockTrade}");
            return RedirectToAction(nameof(Order));
        }

        [HttpPost]
        [Route("SellOrder")]
        [TypeFilter(typeof(ActionFilterForModelValidation))]
        public async Task<IActionResult> SellOrder(StockTrade stockTrade)
        {
            _logger.LogInformation($"Sell Order Request {stockTrade}");
            //if (!ModelState.IsValid)
            //{
            //    foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
            //    {
            //        Console.WriteLine(error.ErrorMessage);
            //    }

            //    ViewBag.ErrorMessage = "There was an error with your input. Please correct it and try again.";
            //    return View("Index", stockTrade);
            //}
            SellOrderRequest sellOrderRequest = stockTrade.ToSellOrderRequest();
            sellOrderRequest.DateAndTimeOfOrder = DateTime.Now;
            await _stocksService.CreateSellOrder(sellOrderRequest);
            _logger.LogInformation($"End of Sell Order Request {stockTrade}");
            return RedirectToAction(nameof(Order));
        }

        [HttpGet]
        [Route("Order")]
        public async Task<IActionResult> Order()
        {
            _logger.LogInformation("In Orders Page action");
            try
            {
                Orders orders = new Orders()
                {
                    BuyOrders = await _stocksService.GetBuyOrders(),
                    SellOrders = await _stocksService.GetSellOrders()
                };

                _logger.LogInformation("End of Orders Page action");
                return View(orders);
            }
            catch (Exception ex)
            {
                return View();
            }
        }

        [Route("download-pdf")]
        public async Task<IActionResult> DownloadPDF()
        {
            _logger.LogInformation("In DownloadPDF Page action");
            List<BuyOrderResponse>? BuyOrders = await _stocksService.GetBuyOrders();
            List<SellOrderResponse>? SellOrders = await _stocksService.GetSellOrders();

            List<PDFModel> pdfModels = MergeOrders(BuyOrders, SellOrders);
            _logger.LogInformation("End of DownloadPDF Page action");
            return new ViewAsPdf("download-pdf", pdfModels);
        }

        private List<PDFModel> MergeOrders(List<BuyOrderResponse>? buyOrders, List<SellOrderResponse>? sellOrders)
        {
            List<PDFModel> combinedOrders = new List<PDFModel>();

            if (buyOrders != null)
            {
                combinedOrders.AddRange(buyOrders.Select(buyOrder => new PDFModel
                {
                    DateAndTimeOfOrder = buyOrder.DateAndTimeOfOrder,
                    StockName = buyOrder.StockName,
                    StockSymbol = buyOrder.StockSymbol,
                    OrderType = "Buy",
                    Quantity = buyOrder.Quantity,
                    Price = buyOrder.Price,
                    TradeAmount = buyOrder.TradeAmount
                }));
            }

            if (sellOrders != null)
            {
                combinedOrders.AddRange(sellOrders.Select(sellOrder => new PDFModel
                {
                    DateAndTimeOfOrder = sellOrder.DateAndTimeOfOrder,
                    StockName = sellOrder.StockName,
                    StockSymbol = sellOrder.StockSymbol,
                    OrderType = "Sell",
                    Quantity = sellOrder.Quantity,
                    Price = sellOrder.Price,
                    TradeAmount = sellOrder.TradeAmount
                }));
            }

            return combinedOrders.OrderByDescending(temp=>temp.DateAndTimeOfOrder).ToList();
        }
    }
}
