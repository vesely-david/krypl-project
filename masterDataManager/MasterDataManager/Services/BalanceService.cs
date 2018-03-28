using DataLayer.Enums;
using DataLayer.Infrastructure.Interfaces;
using DataLayer.Models;
using MasterDataManager.Models;
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

        public void UpdateUserAssets(IEnumerable<Asset> assets, int userId, int exchangeId, TradingMode tradingMode)
        {
            var userAssets = _userAssetRepository.GetByUserAndExchange(userId, exchangeId).Where(o => o.TradingMode == tradingMode);

            var toDelete = userAssets.Where(o => !assets.Any(p => p.CurrencyId == o.CurrencyId));

            foreach (var userAsset in toDelete) DeleteUserAsset(userAsset);

            foreach(var asset in assets)
            {
                var userAsset = userAssets.FirstOrDefault(o => o.CurrencyId == asset.CurrencyId);
                if (userAsset == null)
                {
                    if (asset.Amount <= 0) continue;
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
                    if (asset.Amount <= 0)
                    {
                        DeleteUserAsset(userAsset);
                    }
                    else
                    {
                        userAsset.Amount = asset.Amount;
                        _userAssetRepository.EditNotSave(userAsset);
                    }
                }
            }
            _userAssetRepository.Save();
        }

        private void DeleteUserAsset(UserAsset userAsset)
        {
            // Preserve asset for running strategies
            if (userAsset.StrategyAssets != null && userAsset.StrategyAssets.Any())
            {
                userAsset.Amount = 0;
                _userAssetRepository.EditNotSave(userAsset);
            } 
            _userAssetRepository.DeleteNotSave(userAsset);
        }
    }
}
