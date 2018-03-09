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
        public Task<List<Asset>> GetBalances(int userId)
        {
            throw new NotImplementedException();
        }

        public int GetExchangeId()
        {
            throw new NotImplementedException();
        }
    }
}
