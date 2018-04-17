from trading.money.transaction import SellTransaction, BuyTransaction


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
        self.transactions.append(transaction)
        self.add(transaction.gainedContract())
        self.add(transaction.subtractedContract() * (-1) - transaction.fee)

    def sell(self, contractPair, amount, price):
        transaction = SellTransaction(contractPair, self.timeServer.time, amount, price, self.absoluteFee(amount, price))
        self.transactions.append(transaction)
        self.add(transaction.gainedContract() - transaction.fee)
        self.add(transaction.subtractedContract() * (-1))

    def balance(self, contract):
        return self.wallet.get(contract, 0.)

    # --- private ---
    def absoluteFee(self, amount, price):
        return amount * price * self.fee

    def add(self, contract):
        self.wallet[contract.name] = self.wallet.get(contract.name, 0.) + contract.value
