using DataLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MasterDataManager.Services.ServiceModels
{
    public class Asset
    {
        public string Currency { get; set; }
        public decimal Amount { get; set; }
    }
}
