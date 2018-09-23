using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MasterDataManager.Services.ServiceModels
{
    public class BinanceAccountInfo
    {
        public int makerCommission { get; set; }
        public int takerCommission { get; set; }
        public int buyerCommission { get; set; }
        public int sellerCommission { get; set; }
        public bool canTrade { get; set; }
        public bool canWithdraw { get; set; }
        public bool canDeposit { get; set; }
        public long updateTime { get; set; }
        public IEnumerable<BinanceAsset> balances { get; set; }
    }
}
