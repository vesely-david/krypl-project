using System;
namespace MasterDataManager.Services.ServiceModels
{
    public class PoloniexTrade
    {
        public string currencyPair { get; set; }
        public string status { get; set; }
        public DateTime date { get; set; }
        public string type { get; set; }
        public string rate { get; set; }
        public string amount { get; set; }
        public string total { get; set; }
    }
}
