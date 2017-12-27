from trading.exchange import BackTestExchange
from test.trading.money.test_money import TestMoney
from trading.money.transaction import BuyTransaction, SellTransaction


class TestBackTestExchange(TestMoney):
    class TestTimeServer:
        def __init__(self):
            self.time = 0

    def assertTransactions(self, transactions, expectedTransactions):
        for t, e in zip(transactions, expectedTransactions):
            self.assertEqual(t.contractPair, e.contractPair)
            self.assertEqual(t.amount, e.amount)
            self.assertEqual(t.price, e.price)
            self.assertEqual(t.fee, e.fee)

    def test_buy(self):
        exchange = BackTestExchange(self.TestTimeServer(), wallet={'czk': 10, 'btc': 11}, fee=0.5)
        exchange.buy(self.czkBtcContract(), 1, 2)
        self.assertEqual(exchange.balance('czk'), 7)
        self.assertEqual(exchange.balance('btc'), 12)

        expectedTransactions = [BuyTransaction(self.czkBtcContract(), 0, 1, 2, 1)]
        self.assertTransactions(exchange.transactions, expectedTransactions)

    def test_sell(self):
        exchange = BackTestExchange(self.TestTimeServer(), wallet={'czk': 10, 'btc': 11}, fee=0.5)
        exchange.sell(self.czkBtcContract(), 1, 2)
        self.assertEqual(exchange.balance('czk'), 11)
        self.assertEqual(exchange.balance('btc'), 10)

        expectedTransactions = [SellTransaction(self.czkBtcContract(), 0, 1, 2, 1)]
        self.assertTransactions(exchange.transactions, expectedTransactions)

    def test_balance(self):
        exchange = BackTestExchange(self.TestTimeServer, wallet={'czk': 10., 'btc': 11.})
        self.assertEqual(exchange.balance('btc'), 11.)
