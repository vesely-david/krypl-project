using MasterDataManager.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataLayer.Models;
using System.Net.Http;
using Newtonsoft.Json;
using MasterDataManager.Services.ServiceModels;
using MasterDataManager.Utils;
using DataLayer.Enums;

namespace MasterDataManager.Services
{
    public class BinanceWrapper
    {
        private HttpClient _client;
        private readonly string _baseEndpoint = "https://api.binance.com";

        public BinanceWrapper()
        {
            _client = new HttpClient();
        }

        public async Task<List<BinanceAsset> > GetBalances(string apiKey, string apiSecret)
        {
            var url = _baseEndpoint + "/api/v3/account";
            var response = await _client.BinanceSignedRequest<BinanceAccountInfo>(url, HttpMethod.Get, null, apiKey, apiSecret);
            return response.balances.ToList();
        }

        public async Task<string> PutOrder(string apiKey, string apiSecret, TradeOrder order, OrderType type)
        {
            var url = _baseEndpoint + "/api/v3/order";
            var response = await _client.BinanceSignedRequest<BinanceOrderResult>(url, HttpMethod.Post, new Dictionary<string, string> {
                {"side", type == OrderType.Buy ? "BUY": "SELL"},
                {"symbol", order.Symbol},
                {"quantity", order.Amount.ToString()},
                {"price", order.Rate.ToString()},
                {"type", "LIMIT"}
            }, apiKey, apiSecret);
            return response.orderId;
        }
    }
}
