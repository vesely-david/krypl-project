using DataLayer.Services;
using MarketDataProvider.Services.MarketDataProviders;
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
        private MarketDataService _dataProvider;

        private int countCounter = 0;


        public DataRefreshService(
            PriceService priceProvider,
            MarketDataService dataProvider)
        {
            _priceProvider = priceProvider;
            _dataProvider = dataProvider;
        }
        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                //LATER Cancellation token aftex 15sec or so?
                try
                {
                    if(countCounter % 60 == 0)
                    {
                        await _dataProvider.UpdateInfo(cancellationToken);
                        countCounter = 0;
                    }

                    await _priceProvider.UpdatePrices(cancellationToken);
                    countCounter++;
                } catch(Exception e) { Console.WriteLine(e.InnerException); }

                await Task.Delay(TimeSpan.FromMinutes(1), cancellationToken);
            }
        }
    }
}
