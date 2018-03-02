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
    public class BittrexService : IExchangeService
    {

        public List<UserAsset> GetBalances()
        {
            //https://bittrex.com/api/v1.1/account/getbalances?apikey=API_KEY
            throw new NotImplementedException();
        }

        Task<List<UserAsset>> IExchangeService.GetBalances()
        {
            throw new NotImplementedException();
        }
    }
}
