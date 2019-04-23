from trading.money.transaction import Transaction
from trading.money.contract import Contract
from copy import deepcopy


class Exchange:
    def buy(self, contractPair, amount, price):
        raise NotImplementedError()

    def sell(self, contractPair, amount, price):
        raise NotImplementedError()

    def balance(self, currency):
        raise NotImplementedError()


class BackTestExchange(Exchange):
    def __init__(self, time_server, wallet={}, fee=0.):
        self.wallet = wallet
        self.transactions = []
        self.fee = fee
        self.time_server = time_server
        self.wallet_history = {0: deepcopy(wallet)}

    def buy(self, pair, amount, price):
        transaction = Transaction.buy(pair, self.time_server.time, amount, price, self._absolute_fee(amount, price))
        subtracted = Contract.add(Transaction.subtracted_contract(transaction), transaction['fee'])
        self._assert_enough_in_wallet(subtracted)

        self.transactions.append(transaction)
        self._add(Transaction.gained_contract(transaction))
        self._subtract(subtracted)
        self.wallet_history[transaction['timestamp']] = deepcopy(self.wallet)

    def sell(self, pair, amount, price):
        transaction = Transaction.sell(pair, self.time_server.time, amount, price, self._absolute_fee(amount, price))
        subtracted = Transaction.subtracted_contract(transaction)
        self._assert_enough_in_wallet(subtracted)

        self.transactions.append(transaction)
        gained = Contract.sub(Transaction.gained_contract(transaction), transaction['fee'])
        self._add(gained)
        self._subtract(subtracted)
        self.wallet_history[transaction['timestamp']] = deepcopy(self.wallet)

    def balance(self, contract_name):
        return self.wallet.get(contract_name, 0.)

    # --- private ---
    def _absolute_fee(self, amount, price):
        return amount * price * self.fee

    def _add(self, contract):
        name, value = contract
        self.wallet[name] = self.wallet.get(name, 0.) + value

    def _subtract(self, contract):
        self._add(Contract.mul(contract, -1))

    def _assert_enough_in_wallet(self, contract):
        name, value = contract
        if self.balance(name) < value:
            raise ValueError()
