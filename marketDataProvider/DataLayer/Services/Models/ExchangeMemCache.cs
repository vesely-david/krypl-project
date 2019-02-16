using DataLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataLayer.Services.Models
{
    public class ExchangeMemCache
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Web { get; set; }
        public bool ProvidesFullHistoryData { get; set; }

        public virtual ICollection<MarketMemCache> Markets { get; private set; }
        public virtual ICollection<CurrencyMemCache> Currencies { get; private set; }
        private Dictionary<string, string> MarketTranslator;
        private Dictionary<string, string> CurrencyTranslator;

        public ExchangeMemCache(ICollection<MarketMemCache> markets, ICollection<CurrencyMemCache> currencies)
        {
            Markets = markets;
            Currencies = currencies;
            MarketTranslator = markets.ToDictionary(o => o.MarketExchangeId, o => o.Id);
            CurrencyTranslator = currencies.ToDictionary(o => o.CurrencyExchangeId, o => o.Id);

        }

        public bool TranslateMarket(string exchangeMarket, out string market)
        {
            market = "";
            if (MarketTranslator.ContainsKey(exchangeMarket))
            {
                market = MarketTranslator[exchangeMarket];
                return true;
            }
            return false;
        }

        public bool TranslateCurrency(string exchangeCurrency, out string currency)
        {
            currency = "";
            if (CurrencyTranslator.ContainsKey(exchangeCurrency))
            {
                currency = CurrencyTranslator[exchangeCurrency];
                return true;
            }
            return false;
        }
    }
}