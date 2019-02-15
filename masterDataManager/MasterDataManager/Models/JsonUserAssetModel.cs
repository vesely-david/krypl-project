using System;
namespace MasterDataManager.Models
{
    public class JsonUserAssetModel : JsonAssetModel
    {
        public decimal free { get; set; }
        public string tradingMode { get; set; }
    }
}
