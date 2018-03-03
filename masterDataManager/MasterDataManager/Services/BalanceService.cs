using DataLayer.Enums;
using DataLayer.Infrastructure.Interfaces;
using DataLayer.Models;
using MasterDataManager.Services.Interfaces;
using MasterDataManager.Services.ServiceModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MasterDataManager.Services
{
    public class BalanceService : IBalanceService
    {
        private IUserAssetRepository _userAssetRepository;

        public BalanceService(
            IUserAssetRepository userAssetRepository)
        {
            _userAssetRepository = userAssetRepository;
        }

        public bool UpdateUserAssets(List<Asset> assets, int userId, int exchangeId, TradingMode tradingMode, out List<string> insufficient)
        {
            insufficient = new List<string>();
            var userAssets = _userAssetRepository.GetByUserId(userId).Where(o => o.TradingMode == tradingMode);

            foreach(var asset in assets)
            {
                var userAsset = userAssets.FirstOrDefault(o => o.CurrencyId == asset.CurrencyId);
                if (userAsset == null)
                {
                    _userAssetRepository.AddNotSave(new UserAsset
                    {
                        Amount = asset.Amount,
                        CurrencyId = asset.CurrencyId,
                        ExchangeId = exchangeId,
                        UserId = userId,
                        StrategyAssets = new List<StrategyAsset>(),
                        TradingMode = tradingMode,
                    });
                }
                else
                {
                    userAsset.Amount = asset.Amount;
                    _userAssetRepository.EditNotSave(userAsset);
                    var freeAmount = userAsset.GetFreeAmount();
                    if (freeAmount < 0) insufficient.Add(userAsset.Currency.Code + " : " + freeAmount);
                }
            }
            _userAssetRepository.Save();
            return true;
        } 

    }
}
