from enum import Enum

class Contract:
    def __init__(self, name, value):
        self.name = name
        self.value = value


class Wallet:
    def __init__(self):
        self.money = dict()

    def addContract(self, contract):
        raise NotImplementedError()

    def addTransaction(selfs, transaction):
        raise NotImplementedError()


class Transaction:
    Type = Enum('Type', 'buy sell')

    def __init__(self, timestamp, amount, price, type, feePerc):
        raise NotImplementedError()
        # self.timestamp = None
        # self.amount = None
        # self.price = None
        # self.type = None
        # self.fee = None


class Trade:

    Type = Enum('Type', 'long short')

    def __init__(self):
        raise NotImplementedError()

    def open(self, price):
        raise NotImplementedError()

    def close(self, price):
        raise NotImplementedError()

    def isClosed(self):
        raise NotImplementedError()

    def totalProfit(self):
        raise NotImplementedError()

    def relativeProfit(self):
        raise NotImplementedError()
