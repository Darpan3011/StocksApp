using Microsoft.AspNetCore.Mvc;
using ServiceContracts;

namespace UnitTesting_UDEMY.ViewComponents
{
    public class CompanyProfileViewComponent : ViewComponent
    {
        private readonly IFinnhubService _finnhubService;
        public CompanyProfileViewComponent(IFinnhubService finnhubService) {
            _finnhubService = finnhubService;
        }
        public async Task<IViewComponentResult> InvokeAsync(string? stockSymbol)
        {
            Dictionary<string, object>? companyProfileDict = null;

            if (stockSymbol != null)
            {
                companyProfileDict = await _finnhubService.GetCompanyProfile(stockSymbol);
                var stockPriceDict = await _finnhubService.GetStockPriceQuote(stockSymbol);
                if (stockPriceDict != null && companyProfileDict != null)
                {
                    companyProfileDict.Add("price", stockPriceDict["c"]);
                }
            }

            if (companyProfileDict != null && companyProfileDict.ContainsKey("logo"))
                return View(companyProfileDict);
            else
                return Content("");
        }
    }
}
