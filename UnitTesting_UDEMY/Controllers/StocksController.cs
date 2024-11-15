using Microsoft.AspNetCore.Mvc;
using ServiceContracts;

namespace UnitTesting_UDEMY.Controllers
{
    [Route("[controller]")]
    public class StocksController : Controller
    {
        private readonly IFinnhubService _finnhubService;
        private readonly IConfiguration _configuration;

        public StocksController(IFinnhubService finnhubService, IConfiguration configuration)
        {
            _finnhubService = finnhubService;
            _configuration = configuration;
        }

        [Route("[action]")]
        public async Task<IActionResult> Explore(string? stock)
        {
            bool onlyTop25 = true;
            List<Dictionary<string, string>>? listofStocks = await _finnhubService.getAllStocks();

            if (onlyTop25)
            {
                var top25PopularStocks = _configuration["TradingOptions:Top25PopularStocks"]!
                    .Split(',')
                    .Select(s => s.Trim())
                    .ToHashSet();

                listofStocks = listofStocks?
                    .Where(s => top25PopularStocks.Contains(s["displaySymbol"]))
                    .ToList();

            }

            ViewBag.stock = stock;
            return View(listofStocks);
        }

        [HttpGet("LoadCompanyProfile/{stockSymbol?}")]
        public Task<IActionResult> LoadCompanyProfile(string stockSymbol)
        {
            return Task.FromResult<IActionResult>(ViewComponent("CompanyProfile", new { stockSymbol }));
        }

    }
}
