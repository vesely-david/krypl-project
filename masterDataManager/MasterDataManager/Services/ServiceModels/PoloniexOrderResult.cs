using System;
namespace MasterDataManager.Services.ServiceModels
{
    public class PoloniexOrderResult
    {
        public string orderNumber { get; set; }
        public object resultingTrades { get; set; }
    }
}
