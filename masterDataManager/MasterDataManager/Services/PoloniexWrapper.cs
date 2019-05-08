
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using DataLayer.Enums;
using DataLayer.Models;
using MasterDataManager.Services.ServiceModels;
using MasterDataManager.Utils;

namespace MasterDataManager.Services
{
    public class PoloniexWrapper
    {
        private HttpClient _client;
        private readonly string _baseEndpoint = "https://poloniex.com/tradingApi";

        public PoloniexWrapper()
        {
            _client = new HttpClient();
        }

        public async Task<List<PoloniexBalance>> GetBalances(string apiKey, string apiSecret)
        {;
            var response = await _client.PoloniexSignedRequest<Dictionary<string, decimal>>(_baseEndpoint, HttpMethod.Post, new Dictionary<string, string> { {"command", "returnBalances" } }, apiKey, apiSecret);
            return response.Where(o => o.Value > 0m).Select(o => new PoloniexBalance
            {
                amount = o.Value,
                asset = o.Key,
            }).ToList();
        }

        public async Task<string> PutOrder(string apiKey, string apiSecret, TradeOrder order, OrderType type)
        {
            var response = await _client.PoloniexSignedRequest<PoloniexOrderResult>(_baseEndpoint, HttpMethod.Post, new Dictionary<string, string> {
                {"command", type == OrderType.Buy ? "buy": "sell"},
                {"currencyPair", order.Symbol},
                {"amount", order.Amount.ToString()},
                {"rate", order.Rate.ToString()},
            }, apiKey, apiSecret);
            return response.orderNumber;
        }


        public async Task CancelOrder(string apiKey, string apiSecret, string orderId)
        {
            var response = await _client.PoloniexSignedRequest<object>(_baseEndpoint, HttpMethod.Post, new Dictionary<string, string> {
                {"command", "cancelOrder"},
                {"orderNumber", orderId},
            }, apiKey, apiSecret);
        }


        public async Task<PoloniexTrade> GetTrade(string apiKey, string apiSecret, string tradeId)
        {
            var response = await _client.PoloniexSignedRequest(_baseEndpoint, HttpMethod.Post, new Dictionary<string, string> {
                {"command", "returnOrderStatus"},
                {"orderNumber", tradeId},
            }, apiKey, apiSecret);

            var trade = response.SelectToken("result." + tradeId).ToObject(typeof(PoloniexTrade));

            return (PoloniexTrade)trade;
        }
    }
}
