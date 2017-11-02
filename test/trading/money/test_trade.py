from .test_money import TestMoney
from trading.money.trade import Trade
from trading.money.transaction import Transaction


class TestTrade(TestMoney):
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
                                     transactionType=Transaction.Type.buy, fee=self.czk(10.0))
        self.assertEqual(transaction, transactionRes)
        self.assertEqual(self.trade, self.openedLong)

    def test_openShort(self):
        transaction = self.trade.open(price=self.price, tradeType=Trade.Type.short, stopLoss=self.czk(102.0),
                                      targetPrice=self.czk(95.0), fee=self.czk(10.0))
        transactionRes = Transaction(transaction.timestamp, amount=self.tradeSubject, price=self.price,
                                     transactionType=Transaction.Type.sell, fee=self.czk(10.0))
        self.assertEqual(transaction, transactionRes)
        self.assertEqual(self.trade, self.openedShort)

    def test_closeLong(self):
        transaction = self.openedLong.close(price=self.czk(106.0), fee=self.czk(10.6))
        transactionRes = Transaction(transaction.timestamp, self.openedLong.tradeSubject, self.czk(106.0),
                                     Transaction.Type.sell, self.czk(10.6))

        self.assertEqual(transaction, transactionRes)
        self.assertEqual(self.openedLong, self.closedLong)

    def test_closeShort(self):
        transaction = self.openedLong.close(price=self.czk(92.0), fee=self.czk(9.2))
        transactionRes = Transaction(transaction.timestamp, self.openedLong.tradeSubject, self.czk(106.0),
                                     Transaction.Type.buy, self.czk(9.2))

        self.assertEqual(transaction, transactionRes)
        self.assertEqual(self.openedLong, self.closedShort)

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
