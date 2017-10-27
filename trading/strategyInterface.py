from trading.balance import PairBalance


class EntryPoint:
    def entryLong(self):
        raise NotImplementedError()

    def entryShort(self):
        raise NotImplementedError()


class Risk:
    def tradingAmount(self, balance):
        raise NotImplementedError()

    def stopLoss(self, buyFee, sellFee, entryPrice):
        raise NotImplementedError()

    def profitPrice(self, buyFee, sellFee, entryPrice):
        raise NotImplementedError()

    def stopLossAndProfit(self, buyFee, sellFee, entryPrice):
        return self.stopLoss(buyFee, sellFee, entryPrice), self.profitPrice(buyFee, sellFee, entryPrice)


class StrategyManager:
    def __init__(self, strategy, riskManager, balance):
        """
        Parameters
        ----------
        strategy : EntryPoint
            Implemented EntryPoint which strategy uses
        riskManager : Risk
            Implemented Risk to control money
        balance : PairBalance
            The start balance of trading
        """
        self.trades = []
        self.strategy = strategy
        self.riskManager = riskManager
        self.startBalance = balance
        self.actualBalance = PairBalance(balance.primaryContract.name, 0, balance.secondaryContract.name)

    def trade(self, last, bid, ask, history):
        raise NotImplementedError()

    def report(self):
        raise NotImplementedError()
