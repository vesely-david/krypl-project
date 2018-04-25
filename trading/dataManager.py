class DataManager:

    def tick(self, historyLen):
        raise NotImplementedError()


class CurrencyDataManager(DataManager):

    # expected prices to be pandas dataframe with columns: timestamp, open, close, high, low, volume
    def __init__(self, prices, priceCol='close'):
        # cols = ['timestamp', 'open', 'close', 'high', 'low', 'volume']
        self.prices = prices
        self.priceCol = priceCol
        self.time = 1

    def tick(self, historyLen):
        if not self.hasTick():
            return None, None
        price = self.prices.ix[self.time, self.priceCol]
        first = self.time - historyLen - 1 if self.time - historyLen - 1 > 0 else 0
        last = self.time - 1
        history = self.prices.iloc[first:last, :]

        self.time += 1
        return history, price

    def hasTick(self):
        return self.time < self.prices.shape[0]
