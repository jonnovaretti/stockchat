using Jobsity.StockChat.Application.Models;
using Jobsity.StockChat.Application.Models.Factories;
using Jobsity.StockChat.Application.Settings;
using System.Net.Http;
using System.Threading.Tasks;

namespace Jobsity.StockChat.Application.Services
{
    public class StockRequestService : IStockRequestService
    {
        private readonly HttpClient _httpClient;
        private readonly IStooqSetting _stooqSetting;

        public StockRequestService(IHttpClientFactory httpClientFactory, IStooqSetting stooqSetting)
        {
            _httpClient = httpClientFactory.CreateClient();
            _stooqSetting = stooqSetting;
        }

        public async Task<StockQuote> Request(string symbol)
        {
            var response = await _httpClient.GetAsync(string.Concat(_stooqSetting.Url, symbol));

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return StockQuoteFactory.Create(content);
            }

            throw new HttpRequestException("An error has occored when trying to get stock quote");
        }
    }
}
