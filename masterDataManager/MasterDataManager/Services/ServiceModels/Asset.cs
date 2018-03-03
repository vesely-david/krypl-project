using DataLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MasterDataManager.Services.ServiceModels
{
    public class Asset
    {
        public Currency Currency { get; set; }
        public int CurrencyId { get; set; }
        public double Amount { get; set; }
    }
}
