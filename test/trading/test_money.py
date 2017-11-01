from unittest import TestCase

from trading.money import Contract, Wallet, Transaction, Trade


class TestContract(TestCase):
    def test___mul__(self):
        self.fail()

    def test___rmul__(self):
        self.fail()


class TestMoney(TestCase):
    @staticmethod
    def czk(value):
        return Contract('czk', value)

    @staticmethod
    def btc(value):
        return Contract('btc', value)


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


class TestTrade(TestCase):
    def setUp(self):
        self.tradeSubject = self.btc(10.0)
        self.price = self.czk(100.0)
        self.trade = Trade(tradeSubject=self.tradeSubject)
        self.openedLong = Trade(tradeSubject=self.tradeSubject, tradeType=Trade.Type.long, openPrice=self.price,
                                openFee=self.czk(10.0), stopLoss=self.czk(98.0), targetPrice=self.czk(105.0),
                                closePrice=None,
                                closeFee=None)

        self.openedShort = Trade(tradeSubject=self.tradeSubject, tradeType=Trade.Type.short, openPrice=self.price,
                                 openFee=self.czk(10.0), stopLoss=self.czk(102.0), targetPrice=self.czk(95.0),
                                 closePrice=None,
                                 closeFee=None)

        self.closedLong = Trade(tradeSubject=self.tradeSubject, tradeType=Trade.Type.long, openPrice=self.price,
                                openFee=self.czk(10.0), stopLoss=self.czk(98.0), targetPrice=self.czk(105.0),
                                closePrice=self.czk(106.0),
                                closeFee=self.czk(10.6))

        self.closedShort = Trade(tradeSubject=self.tradeSubject, tradeType=Trade.Type.short, openPrice=self.price,
                                 openFee=self.czk(10.0), stopLoss=self.czk(102.0), targetPrice=self.czk(95.0),
                                 closePrice=self.czk(92.0),
                                 closeFee=self.czk(10.6))

    def test_openLong(self):
        transaction = self.trade.open(price=self.price, tradeType=Trade.Type.long, stopLoss=self.czk(98.0),
                                      targetPrice=self.czk(105.0), fee=self.czk(10.0))
        transactionRes = Transaction(transaction.timestamp, amount=self.tradeSubject, price=self.price,
                                     type=Transaction.Type.buy, fee=self.czk(10.0))
        self.assertEqual(transaction, transactionRes)
        self.assertEqual(self.trade, self.openedLong)

    def test_openShort(self):
        transaction = self.trade.open(price=self.price, tradeType=Trade.Type.short, stopLoss=self.czk(102.0),
                                      targetPrice=self.czk(95.0), fee=self.czk(10.0))
        transactionRes = Transaction(transaction.timestamp, amount=self.tradeSubject, price=self.price,
                                     type=Transaction.Type.sell, fee=self.czk(10.0))
        self.assertEqual(transaction, transactionRes)
        self.assertEqual(self.trade, self.openedShort)

    def test_closeLong(self):
        transaction = self.openedLong.close(price=self.czk(106.0), fee=self.czk(10.6))
        transactionRes = Transaction(transaction.timestamp, self.openedLong.tradeSubject, self.czk(106.0),
                                     Transaction.type.sell, self.czk(10.6))

        self.assertEqual(transaction, transactionRes)
        self.assertEqual(self.openedLong, self.closedLong)

    def test_closeShort(self):
        transaction = self.openedLong.close(price=self.czk(92.0), fee=self.czk(9.2))
        transactionRes = Transaction(transaction.timestamp, self.openedLong.tradeSubject, self.czk(106.0),
                                     Transaction.type.buy, self.czk(9.2))

        self.assertEqual(transaction, transactionRes)
        self.assertEqual(self.openedLong, self.clsedShort)

    def test_isClosed(self):
        self.assertEqual(self.openedLong, False)
        self.assertEqual(self.openedLong.close(price=self.czk(106.0), fee=self.czk(10.6)), True)
        self.assertEqual(self.openedShort, False)
        self.assertEqual(self.openedShort.close(price=self.czk(106.0), fee=self.czk(10.6)), True)

    def test_totalProfitShort(self):
        profit = self.closedShort.totalProfit()
        self.assertEqual(profit, -0.6)

    def test_totalProfitLong(self):
        profit = self.closedLong.totalProfit()
        self.assertEqual(profit, 39.4)

    def test_totalProfit_openedTradeShouldFail(self):
        with self.assertRaises(ValueError):
            self.openedLong.totalProfit()

    def test_relativeProfitShort(self):
        profit = self.closedShort.relativeProfit()
        self.assertEqual(profit, -0.0006)

    def test_relativeProfiLong(self):
        profit = self.closedLong.relativeProfit()
        self.assertEqual(profit, 0.0394)

    def test_relativeProfit_openedTradeShouldFail(self):
        with self.assertRaises(ValueError):
            self.openedLong.relativeProfit()

