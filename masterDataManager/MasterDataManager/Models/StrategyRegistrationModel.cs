using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MasterDataManager.Models
{
    public class StrategyRegistrationModel
    {
        public string name { get; set; }
        public string exchange { get; set; }
        public string description { get; set; }
        public List<AssetModel> assets { get; set; }
    }
}
