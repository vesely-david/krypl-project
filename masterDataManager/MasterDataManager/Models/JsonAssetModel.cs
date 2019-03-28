using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MasterDataManager.Models
{
    public class JsonAssetModel
    {
        public string id { get; set; }
        public string exchange { get; set; }
        public string currency { get; set; }
        public decimal amount { get; set; }
        public string strategyId { get; set; }
        public string tradingMode { get; set; }
        public bool isActive { get; set; }
        public bool isReserved { get; set; }
    }
}
