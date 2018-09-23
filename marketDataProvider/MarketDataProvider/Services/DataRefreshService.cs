using DataLayer.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MarketDataProvider.Services
{
    public class DataRefreshService : HostedService
    {
        private PriceService _priceProvider;

        public DataRefreshService(PriceService priceProvider)
        {
            _priceProvider = priceProvider;
        }
        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                //LATER Cancellation token aftex 15sec or so?
                await _priceProvider.UpdatePrices(cancellationToken);
                await Task.Delay(TimeSpan.FromMinutes(1), cancellationToken);
            }
        }
    }
}
