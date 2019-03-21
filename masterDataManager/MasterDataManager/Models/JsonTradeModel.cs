using System;
namespace MasterDataManager.Models
{
    public class JsonTradeModel
    {
        public DateTime opened { get; set; }
        public DateTime closed { get; set; }
        public string tradeState { get; set; }
        public string market { get; set; }
        public string type { get; set; }
        public decimal rate { get; set; }
        public decimal total { get; set; }
        public decimal volume { get; set; }
    }
}
