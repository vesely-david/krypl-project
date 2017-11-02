from enum import Enum


class Trade:
    Type = Enum('Type', 'long short')

    def __init__(self, tradeSubject, tradeType=None, openPrice=None, openFee=None, stopLoss=None, targetPrice=None,
                 closePrice=None, closeFee=None):
        raise NotImplementedError()

    def open(self, price, tradeType, stopLoss, targetPrice, fee):
        raise NotImplementedError()

    def close(self, price, fee):
        raise NotImplementedError()

    def isClosed(self):
        raise NotImplementedError()

    def totalProfit(self):
        raise NotImplementedError()

    def relativeProfit(self):
        raise NotImplementedError()
