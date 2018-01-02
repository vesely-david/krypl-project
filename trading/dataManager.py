class DataManager:

    def tick(self, historyLen):
        raise NotImplementedError()


class CurrencyDataManager(DataManager):

    # expected prices to be pandas dataframe with columns: timestamp, open, close, high, low, volume
    def __init__(self, prices):
        # cols = ['timestamp', 'open', 'close', 'high', 'low', 'volume']
        self.prices = prices
        self.time = 1

    def tick(self, historyLen):
        if not self.hasTick():
            return None, None
        price = self.prices.ix[self.time, 'close']
        first = self.time - historyLen if self.time - historyLen > 0 else 0
        last = self.time - 1
        history = self.prices.ix[first:last, :]

        self.time += 1
        return history, price

    def hasTick(self):
        return self.time < self.prices.shape[0]
