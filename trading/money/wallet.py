class Wallet:
    def __init__(self, moneyDict=None):
        self.money = {} if moneyDict is None else moneyDict

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