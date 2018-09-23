using DataLayer.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Models
{
    public class StrategyAsset : IdEntity
    {
        public decimal Amount { get; set; }

        public string UserAssetId { get; set; }
        public virtual UserAsset UserAsset { get; set; }
        public string StrategyId { get; set; }
        public Strategy Strategy { get; set; }
    }
}
