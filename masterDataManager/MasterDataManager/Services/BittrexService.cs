using DataLayer.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataLayer.Models;
using MasterDataManager.Services.Interfaces;
using DataLayer.Enums;

namespace MasterDataManager.Services
{
    public class BittrexService : IBittrexService
    {
        //private String _marketUrl = String.Format(@"https://bittrex.com/api/v1.1/market/{0}?apikey={1}");
        //private String _accountUrl = String.Format(@"https://bittrex.com/api/v1.1/account/{0}?apikey={1}");

        public string BuyOrder(Market market, double quantity, double rate)
        {
            //https://bittrex.com/api/v1.1/market/buylimit?apikey=API_KEY&market=BTC-LTC&quantity=1.2&rate=1.3  
            throw new NotImplementedException();
        }

        public UserAsset GetBalance(Currency currency)
        {
            //https://bittrex.com/api/v1.1/account/getbalance?apikey=API_KEY&currency=BTC  
            throw new NotImplementedException();
        }

        public List<UserAsset> GetBalances()
        {
            //https://bittrex.com/api/v1.1/account/getbalances?apikey=API_KEY
            throw new NotImplementedException();
        }

        public Trade GetOrder(Trade trade)
        {
            //https://bittrex.com/api/v1.1/account/getorder&uuid=0cb4c4e4-bdc7-4e13-8c13-430e587d2cc1 
            //No API key???
            throw new NotImplementedException();
        }

        public string SellOrder(Market market, double quantity, double rate)
        {
            //https://bittrex.com/api/v1.1/market/selllimit?apikey=API_KEY&market=BTC-LTC&quantity=1.2&rate=1.3  
            throw new NotImplementedException();
        }

        public bool CancelOrder(Trade trade)
        {
            //https://bittrex.com/api/v1.1/market/cancel?apikey=API_KEY&uuid=ORDER_UUID  
            throw new NotImplementedException();
        }

    }
}
