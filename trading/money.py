from enum import Enum


class Contract:
    def __init__(self, name, value):
        self.name = name
        self.value = value

    def __mul__(self, other):
        return NotImplemented

    def __rmul__(self, other):
        return NotImplemented


class Wallet:
    def __init__(self, moneyDict=dict()):
        self.money = moneyDict

    def addContract(self, contract):
        self.money[contract.name] = self.money.get(contract.name, 0.0) + contract.value

    def __add__(self, other):
        return NotImplemented

    def __radd__(self, other):
        return NotImplemented

    def __sub__(self, other):
        return NotImplemented

    def __rsub__(self, other):
        return NotImplemented

    def addTransaction(self, transaction):
        raise NotImplementedError()


class Transaction:
    Type = Enum('Type', 'buy sell')

    def __init__(self, timestamp, amount, price, transactionType, fee):
        self.timestamp = timestamp
        self.amount = amount
        self.price = price
        self.type = transactionType
        self.fee = fee


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
