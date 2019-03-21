using System;
using DataLayer.Enums;

namespace MasterDataManager.Models
{
    public class JsonOrderModel
    {
        public string exchange { get; set; }
        public string symbol { get; set; }
        public decimal amount { get; set; }
        public OrderType type { get; set; }
        public decimal? rate { get; set; }
    }
}
