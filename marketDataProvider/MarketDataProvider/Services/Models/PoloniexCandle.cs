using System;
namespace MarketDataProvider.Services.Models
{
    public class PoloniexCandle
    {
        public long date { get; set; }
        public decimal high { get; set; }
        public decimal low { get; set; }
        public decimal open { get; set; }
        public decimal close { get; set; }
    }
}
