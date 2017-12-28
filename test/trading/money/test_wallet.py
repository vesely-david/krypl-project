from ..money.test_money import TestMoney
from trading.money.wallet import Wallet
from trading.money.contract import Contract
from trading.money.transaction import Transaction


class TestWallet(TestMoney):
    def setUp(self):
        self.contract = Contract("c1", 15.0)
        self.emptyWallet = Wallet()
        self.nonEmptyWallet = Wallet({"c1": 2.0, "c2": 5.0})

    def test_addContractToEmptyWallet(self):
        wallet = self.emptyWallet.addContract(self.contract)
        self.assertEqual(wallet, Wallet({"c1": 15.0}))

    def test_addExistingContractToWallet(self):
        wallet = self.nonEmptyWallet.addContract(self.contract)
        self.assertEqual(wallet, Wallet({"c1": 17.0, "c2": 5.0}))

    def test___add__(self):
        self.fail()

    def test___radd__(self):
        self.fail()

    def test___sub__(self):
        self.fail()

    def test___rsub__(self):
        self.fail()

    def test_addBuyTransaction(self):
        transaction = Transaction(timestamp=0, amount=self.btc(10.0), price=self.czk(100.0),
                                  type=Transaction.Type.buy, fee=self.czk(10.0))
        wallet = self.emptyWallet.addTransaction(transaction)
        walletRes = self.emptyWallet.addContract(self.btc(10.0))
        walletRes.addContract(self.czk(-1000.0))
        walletRes.addContract(self.czk(-10.0))
        self.assertEqual(wallet, walletRes)

    def test_addSellTransaction(self):
        transaction = Transaction(timestamp=0, amount=self.btc(10.0), price=self.czk(100.0),
                                  type=Transaction.Type.sell, fee=self.czk(10.0))
        wallet = self.emptyWallet.addTransaction(transaction)
        walletRes = self.emptyWallet.addContract(self.btc(-10.0))
        walletRes.addContract(self.czk(1000.0))
        walletRes.addContract(self.czk(-10.0))
        self.assertEqual(wallet, walletRes)
