using System;
namespace MasterDataManager.Models
{
    public class JsonExchangeSecretModel
    {
        public string id { get; set; }
        public string exchangeId { get; set; }
        public string apiKey { get; set; }
        public string apiSecret { get; set; }
    }
}
