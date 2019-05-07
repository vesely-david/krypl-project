using System;
using System.Collections.Generic;

namespace MarketDataProvider.Services.MarketDataProviders
{
    public class MarketCalEvent
    {
        public int id { get; set; }
        public bool can_occur_before { get; set; }
        public DateTime date_event { get; set; }
        public IEnumerable<MarketCalCoin> coins { get; set; }
        public IEnumerable<MarketCalCategory> categories { get; set; }
        public MarketCalEnTitle title { get; set; }
        public string proof { get; set; }
        public string source { get; set; }
    }
}