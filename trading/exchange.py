from trading.money.transaction import SellTransaction, BuyTransaction
from trading.money.contract import Contract


class Exchange:
    def buy(self, contractPair, amount, price):
        raise NotImplementedError()

    def sell(self, contractPair, amount, price):
        raise NotImplementedError()

    def balance(self, currency):
        raise NotImplementedError()


class BackTestExchange(Exchange):
    def __init__(self, timeServer, wallet={}, fee=0.):
        self.wallet = wallet
        self.transactions = []
        self.fee = fee
        self.timeServer = timeServer

    def buy(self, contractPair, amount, price):
        transaction = BuyTransaction(contractPair, self.timeServer.time, amount, price, self.absoluteFee(amount, price))
        subtractedContract = transaction.subtracted_contract() + transaction.fee
        self._assertEnoughInWallet(subtractedContract)

        self.transactions.append(transaction)
        self.add(transaction.gained_contract())
        self.subtract(subtractedContract)

    def sell(self, contractPair, amount, price):
        transaction = SellTransaction(contractPair, self.timeServer.time, amount, price, self.absoluteFee(amount, price))
        subtractedContract = transaction.subtracted_contract()
        self._assertEnoughInWallet(subtractedContract)

        self.transactions.append(transaction)
        self.add(transaction.gained_contract() - transaction.fee)
        self.subtract(subtractedContract)

    def balance(self, contractName):
        return self.wallet.get(contractName, 0.)

    # --- private ---
    def absoluteFee(self, amount, price):
        return amount * price * self.fee

    def add(self, contract: Contract):
        self.wallet[contract.name] = self.wallet.get(contract.name, 0.) + contract.value

    def subtract(self, contract: Contract):
        self.add(contract * (-1))

    def _assertEnoughInWallet(self, contract: Contract):
        if self.balance(contract.name) < contract.value:
            raise ValueError()
