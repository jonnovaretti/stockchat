using Jobsity.StockChat.Application.Models;
using Jobsity.StockChat.Application.Models.Factories;
using System.Net.Http;
using System.Threading.Tasks;

namespace Jobsity.StockChat.Application.Services
{
    public class StockRequestService : IStockRequestService
    {
        private readonly HttpClient _httpClient;

        public StockRequestService(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
        }

        public async Task<StockQuote> Request(string symbol)
        {
            var response = await _httpClient.GetAsync($"https://stooq.com/q/l/?s={symbol}&f=sd2t2ohlcv&h&e=csv");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return StockQuoteFactory.Create(content);
            }

            throw new HttpRequestException("An error has occored when trying to get stock quote");
        }
    }
}
