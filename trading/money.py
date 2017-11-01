from enum import Enum


class Contract:
    def __init__(self, name, value):
        self.name = name
        self.value = value


class Wallet:
    def __init__(self, moneyDict=dict()):
        self.money = moneyDict

    def addContract(self, contract):
        self.money[contract.name] = self.money.get(contract.name, 0.0) + contract.value

    def addTransaction(self, transaction):
        if transaction.type == Transaction.type.buy:
            self.addContract()


class Transaction:
    Type = Enum('Type', 'buy sell')

    def __init__(self, timestamp, amount, price, type, fee):
        raise NotImplementedError()
        self.timestamp = None
        self.amount = None
        self.price = None
        self.type = None
        self.fee = None


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
