class Statistics:
    def __init__(self):
        self.numOfTrades = 0
        self.totalProfit = .0
        self.wins = 0
        self.losses = 0
        self.lastPeak = 0

    def addTrade(self, trade, balanceBeforeTrade):
        raise NotImplementedError()

    def totalProfit(self):
        raise NotImplementedError()

    def profitPerTrade(self):
        raise NotImplementedError()

    def winPercentage(self):
        raise NotImplementedError()

    def lossPercentage(self):
        raise NotImplementedError()

    def avgWinTrade(self):
        raise NotImplementedError()

    def avgLossTrade(self):
        raise NotImplementedError()

    def profitFactor(self):
        raise NotImplementedError()

    def maxDrawdown(self):
        raise NotImplementedError()
