using DataLayer.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Models
{
    public class StrategyAsset : IdEntity
    {
        public double Amount { get; set; }

        public int UserAssetId { get; set; }
        public virtual UserAsset UserAsset { get; set; }
        public int StrategyId { get; set; }
        public Strategy Strategy { get; set; }
    }
}
