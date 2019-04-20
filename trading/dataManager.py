class DataManager:

    def tick(self, historyLen):
        raise NotImplementedError()


class CurrencyDataManager(DataManager):
    def __init__(self, prices, data):
        self.prices = prices.values
        self.maxTime = self.prices.shape[0]
        self.data = data.values
        self.time = 0

    def tick(self, history_len):
        if not self.has_tick():
            return None, None
        price = self.prices[self.time]
        first = self.time - history_len if self.time - history_len > 0 else 0
        history = self.data[first:self.time, :]

        self.time += 1
        return history, price

    def has_tick(self):
        return self.time < self.maxTime


class OhlcDataManager(DataManager):
    # expected prices to be pandas dataframe with columns: timestamp, open, close, high, low, volume
    def __init__(self, prices, data):
        price_cols = ['open', 'low', 'high', 'close']
        self.prices = prices[price_cols].values
        self.maxTime = self.prices.shape[0]
        self.data = data.values
        self.time = 0
        self.inner_time = 0

    def tick(self, history_len):
        if not self.has_tick():
            return None, None
        price = self.prices[self.time, self.inner_time]
        first = self.time - history_len if self.time - history_len > 0 else 0
        history = self.data[first:self.time, :]
        self.inner_time += 1
        if self.inner_time == 4:
            self.inner_time = 0
            self.time += 1

        return history, price

    def has_tick(self):
        return self.time < self.maxTime
