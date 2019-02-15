using System;
using DataLayer.Enums;

namespace MasterDataManager.Models
{
    public class JsonStrategyOverviewModel
    {
        public string id { get; set; }
        public string name { get; set; }
        public string exchange { get; set; }
        public string description { get; set; }
        public DateTime start { get; set; }
        public DateTime? stop { get; set; }
        public string strategyState { get; set; }

        public decimal initialValueBtc { get; set; }
        public decimal initialValueUsd { get; set; }
        public decimal yesterdayValueBtc { get; set; }
        public decimal yesterdayValueUsd { get; set; }
        public decimal btcValue { get; set; }
        public decimal usdValue { get; set; }
        public int tradesCount { get; set; }
        public int openedTradesCount { get; set; }
        public int newTradesCount { get; set; }
    }
}
