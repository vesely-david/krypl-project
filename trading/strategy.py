import copy


class Strategy:
    def entryLong(self, ask, timestamp, history):
        raise NotImplementedError()

    def entryShort(self, bid, timestamp, history):
        raise NotImplementedError()


class Risk:
    def tradeAmount(self, balance, contractName):
        raise NotImplementedError()

    def stopLoss(self, buyFee, sellFee, entryPrice):
        raise NotImplementedError()

    def profitPrice(self, buyFee, sellFee, entryPrice):
        raise NotImplementedError()

    def stopLossAndProfit(self, buyFee, sellFee, entryPrice):
        return self.stopLoss(buyFee, sellFee, entryPrice), self.profitPrice(buyFee, sellFee, entryPrice)


class MoneyManager:
    def __init__(self, strategy, riskManager, wallet):
        self.transactions = []
        self.trades = []
        self.strategy = strategy
        self.riskManager = riskManager
        self.wallet = wallet
        self.startWallet = copy.deepcopy(wallet)

    def trade(self, data):
        raise NotImplementedError()
