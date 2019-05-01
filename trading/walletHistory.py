from copy import deepcopy
from trading.money.transaction import Transaction
from trading.money.contract import Contract


class WalletHistory:
    def __init__(self, start_wallet, start_ts=0):
        self.start_wallet = deepcopy(start_wallet)
        self.wallet = deepcopy(start_wallet)
        self.wallet_history = {start_ts: self.start_wallet}

    def history(self, transactions):
        for transaction in transactions:
            if transaction['type'] == 'BUY':
                self._buy(transaction)
            else:
                self._sell(transaction)
        return self.wallet_history

    def _buy(self, transaction):
        subtracted = Contract.add(Transaction.subtracted_contract(transaction), transaction['fee'])
        gained = Transaction.gained_contract(transaction)
        self._add(gained)
        self._subtract(subtracted)
        self._add_to_history(self.wallet, transaction['timestamp'])

    def _sell(self, transaction):
        subtracted = Transaction.subtracted_contract(transaction)
        gained = Contract.sub(Transaction.gained_contract(transaction), transaction['fee'])
        self._add(gained)
        self._subtract(subtracted)
        self._add_to_history(self.wallet, transaction['timestamp'])

    def _add_to_history(self, wallet, ts):
        self.wallet_history[ts] = deepcopy(wallet)

    def _add(self, contract):
        name, value = contract
        self.wallet[name] = self.wallet.get(name, 0.) + value

    def _subtract(self, contract):
        self._add(Contract.mul(contract, -1))
