from trading.money.contract import Contract, ContractPair


class Transaction:

    def __init__(self, contractPair: ContractPair, timestamp, amount, price, fee):
        self.contractPair = contractPair
        self.timestamp = timestamp
        self.amount = Contract(contractPair.tradeContract, amount)
        self.price = Contract(contractPair.priceContract, price)
        self.fee = Contract(contractPair.priceContract, fee)

    def gainedContract(self):
        raise NotImplementedError()

    def subtractedContract(self):
        raise NotImplementedError()


class SellTransaction(Transaction):
    def gainedContract(self):
        return self.amount.value * self.price

    def subtractedContract(self):
        return self.amount


class BuyTransaction(Transaction):
    def gainedContract(self):
        return self.amount

    def subtractedContract(self):
        return self.amount.value * self.price
