using System;
using DataLayer.Enums;

namespace MasterDataManager.Services.ServiceModels
{
    public class TradeOrder
    {
        public string Exchange { get; set; }
        public string Symbol { get; set; }
        public decimal Amount { get; set; }
        public decimal? Rate { get; set; }
    }
}