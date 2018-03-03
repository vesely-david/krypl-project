using DataLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MasterDataManager.Services.ServiceModels
{
    public class ExchangeDataDict
    {
        public Dictionary<string, Market> Markets { get; set; }
        public Dictionary<string, Currency> Currencies { get; set; }
        public Exchange Exchange { get; set; }
    }
}
