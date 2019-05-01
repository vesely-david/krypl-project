from unittest import TestCase
from trading.walletHistory import WalletHistory
from trading.money.transaction import Transaction
from trading.money.contract import ContractPair


class TestWalletHistory(TestCase):

    def test_simple_history(self):
        pair = ContractPair.new('btc', 'eth')
        start_wallet = {'btc': 100}
        transactions = [
            Transaction.buy(pair, 1, 10, 1, 1),
            Transaction.sell(pair, 2, 10, 2, 1.5)
        ]
        wallet_history = WalletHistory(start_wallet).history(transactions)

        expected_history = {
            0: start_wallet,
            1: {'btc': 89, 'eth': 10},
            2: {'btc': 107.5, 'eth': 0}
        }

        self.assertDictEqual(expected_history, wallet_history)

    def test_2_pairs_history(self):
        start_wallet = {'btc': 100}
        transactions = [
            Transaction.buy(ContractPair.new('btc', 'eth'), 1, 10, 1, 1),
            Transaction.buy(ContractPair.new('btc', 'xrp'), 1, 5, 1, 1),
            Transaction.sell(ContractPair.new('btc', 'eth'), 2, 5, 2, 1.5),
            Transaction.sell(ContractPair.new('btc', 'xrp'), 3, 5, 2, 0.5),
            Transaction.sell(ContractPair.new('btc', 'eth'), 3, 5, 3, 2),
        ]
        wallet_history = WalletHistory(start_wallet).history(transactions)

        expected_history = {
            0: start_wallet,
            1: {'btc': 83, 'eth': 10, 'xrp': 5},
            2: {'btc': 91.5, 'eth': 5, 'xrp': 5},
            3: {'btc': 114, 'eth': 0, 'xrp': 0}
        }

        self.assertDictEqual(expected_history, wallet_history)
