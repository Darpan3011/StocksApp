using Microsoft.Extensions.Configuration;
using ServiceContracts;
using System.Text.Json;

namespace Services
{
    public class FinnhubService : IFinnhubService
    {
        public readonly IHttpClientFactory _httpClientFactory;
        public readonly IConfiguration _configuration;

        public FinnhubService(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }
        public async Task<Dictionary<string, object>?> GetCompanyProfile(string stockSymbol)
        {
            using (HttpClient httpClient = _httpClientFactory.CreateClient())
            {
                HttpRequestMessage httprequestMessage = new HttpRequestMessage()
                {
                    RequestUri = new Uri($"https://finnhub.io/api/v1/stock/profile2?symbol={stockSymbol}&token={_configuration["token"]}"),
                    Method = HttpMethod.Get
                };
                Console.WriteLine(httprequestMessage.RequestUri);
                HttpResponseMessage httpResponseMessage = await httpClient.SendAsync(httprequestMessage);
                Stream stream = httpResponseMessage.Content.ReadAsStream();
                StreamReader streamReader = new StreamReader(stream);
                string response = streamReader.ReadToEnd();
                Dictionary<string, object>? res = JsonSerializer.Deserialize<Dictionary<string, object>>(response);

                return res;
            }
        }

        public async Task<Dictionary<string, object>?> GetStockPriceQuote(string stockSymbol)
        {
            using (HttpClient httpClient = _httpClientFactory.CreateClient())
            {
                HttpRequestMessage httprequestMessage = new HttpRequestMessage()
                {
                    RequestUri = new Uri($"https://finnhub.io/api/v1/quote?symbol={stockSymbol}&token={_configuration["token"]}"),
                    Method = HttpMethod.Get
                };

                HttpResponseMessage httpResponseMessage = await httpClient.SendAsync(httprequestMessage);
                Stream stream = httpResponseMessage.Content.ReadAsStream();
                StreamReader streamReader = new StreamReader(stream);
                string response = streamReader.ReadToEnd();
                Dictionary<string, object>? res = JsonSerializer.Deserialize<Dictionary<string, object>>(response);

                return res;
            }
        }

        public async Task<List<Dictionary<string, string>>?> getAllStocks()
        {
            HttpClient httpClient = _httpClientFactory.CreateClient();

            //create http request
            HttpRequestMessage httpRequestMessage = new HttpRequestMessage()
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"https://finnhub.io/api/v1/stock/symbol?exchange=US&token={_configuration["token"]}") //URI includes the secret token
            };

            //send request
            HttpResponseMessage httpResponseMessage = await httpClient.SendAsync(httpRequestMessage);

            //read response body
            string responseBody = await httpResponseMessage.Content.ReadAsStringAsync();
            //_diagnosticContext.Set("Response from finnhub", responseBody);

            //convert response body (from JSON into Dictionary)
            List<Dictionary<string, string>>? responseDictionary = JsonSerializer.Deserialize<List<Dictionary<string, string>>>(responseBody);

            if (responseDictionary == null)
                throw new InvalidOperationException("No response from server");

            //return response dictionary back to the caller
            return responseDictionary;
        }

    }
}
