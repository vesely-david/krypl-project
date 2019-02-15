using System;
namespace MasterDataManager.Models
{
    public class JsonTradeModel
    {
        DateTime opened { get; set; }
        DateTime closed { get; set; }
        string tradeState { get; set; }
        string market { get; set; }
        string type { get; set; }
        string status { get; set; }
        decimal price { get; set; }
        decimal volume { get; set; }
    }
}
