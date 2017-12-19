class Statistics:
    def __init__(self, currency, startAmount):
        self.currency = currency
        self.transactions = []

        self.numOfTrades = 0
        self.totalProfit = .0
        self.avgProfit = .0
        self.winningPercentage = .0
        self.avgWinningTrade = .0
        self.avgLosingTrade = .0
        self.profitFactor = .0
        self.maxDrawdown = .0

    def addTransaction(self, transaction):
        raise NotImplementedError()

    def evaluate(self, transactions):
        raise NotImplementedError

    def totalProfit(self):
        raise NotImplementedError()

    def avgProfit(self):
        raise NotImplementedError()

    def winningPercentage(self):
        raise NotImplementedError()

    def avgWinningTrade(self):
        raise NotImplementedError()

    def avgLosingTrade(self):
        raise NotImplementedError()

    def profitFactor(self):
        raise NotImplementedError()

    def maxDrawdown(self):
        raise NotImplementedError()
