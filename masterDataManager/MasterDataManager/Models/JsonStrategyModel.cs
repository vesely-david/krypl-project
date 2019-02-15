using System;

namespace MasterDataManager.Models
{
    public class JsonStrategyModel
    {
        public string id { get; set; }
        public string name { get; set; }
        public string exchange { get; set; }
        public string description { get; set; }
        public string start { get; set; }
        public string stop { get; set; }
        public string strategyState { get; set; }

        public JsonEvaluationModel initialValue { get; set; }
        public JsonEvaluationModel currentValue { get; set; }
        public JsonEvaluationModel yesterdayValue { get; set; }
        public int tradesCount { get; set; }
        public int openedTradesCount { get; set; }
        public int newTradesCount { get; set; }
    }
}
