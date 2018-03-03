using DataLayer.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataLayer.Models;
using MasterDataManager.Services.Interfaces;
using DataLayer.Enums;
using MasterDataManager.Services.ServiceModels;

namespace MasterDataManager.Services
{
    public class BittrexService : IExchangeService
    {

        Task<List<Asset>> IExchangeService.GetBalances()
        {
            //https://bittrex.com/api/v1.1/account/getbalances?apikey=API_KEY

            throw new NotImplementedException();
        }
    }
}
