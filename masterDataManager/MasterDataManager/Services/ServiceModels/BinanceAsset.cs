using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MasterDataManager.Services.ServiceModels
{
    public class BinanceAsset
    {
        public string asset { get; set; }
        public string free { get; set; }
        public string locked { get; set; }
    }
}
