using DataLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MasterDataManager.Services.ServiceModels
{
    public class Asset
    {
        public int CurrencyId { get; set; }
        public virtual Currency Currency { get; set; }
        public double Amount { get; set; }
    }
}
