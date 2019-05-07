using System;
using System.Net;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System.Linq;

namespace MarketDataProvider.Services.MarketDataProviders
{
    public class MarketCal : DataProvider
    {
        private IEnumerable<object> _calEvents = new List<object>();

        public override IEnumerable<object> GetData()
        {
            return _calEvents;
        }

        public override async Task UpdateData()
        {

            var httpRequestMessage = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri("https://developers.coinmarketcal.com/v1/events"),
                Headers = {
                    { "x-api-key", "XsLO9LQig54Ovdk7czvYV1rRL69g7sSHakYUQ63v" },
                    { HttpRequestHeader.Accept.ToString(), "application/json" },
                    { HttpRequestHeader.AcceptEncoding.ToString(), "deflate, gzip" }
                }
            };

            var result = await _client.SendAsync(httpRequestMessage);
            if (result.IsSuccessStatusCode)
            {
                var data = await result.Content.ReadAsStringAsync();
                var events = JObject.Parse(data);

                var calEvents = events.SelectToken("body").ToObject<MarketCalEvent[]>().Select(o => new
                {
                    title = o.title.en,
                    date = o.date_event,
                    coins = o.coins,
                    categories = o.categories,

                });
                _calEvents = calEvents;
            }
            return;
        }
    }
}
