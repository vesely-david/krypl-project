from trading.exchange import BackTestExchange
from test.trading.money.test_money import TestMoney
from trading.money.transaction import Transaction


class TestBackTestExchange(TestMoney):
    class TestTimeServer:
        def __init__(self):
            self.time = 0

    def assert_transactions(self, transactions, expected_transactions):
        for t, e in zip(transactions, expected_transactions):
            self.assertEqual(t, e)

    def test_buy(self):
        exchange = BackTestExchange(self.TestTimeServer(), wallet={'czk': 10, 'btc': 11}, fee=0.5)
        exchange.buy(self.czk_btc(), 1, 2)
        self.assertEqual(exchange.balance('czk'), 7)
        self.assertEqual(exchange.balance('btc'), 12)

        expected = [Transaction.buy(self.czk_btc(), 0, 1, 2, 1)]
        self.assert_transactions(exchange.transactions, expected)

    def test_invalid_buy(self):
        exchange = BackTestExchange(self.TestTimeServer(), wallet={'czk': 10, 'btc': 11}, fee=0.5)
        # should fail, because of fee
        self.assertRaises(ValueError, exchange.buy, self.czk_btc(), 1, 10)
        self.assertEqual(exchange.balance('czk'), 10)
        self.assertEqual(exchange.balance('btc'), 11)

    def test_sell(self):
        exchange = BackTestExchange(self.TestTimeServer(), wallet={'czk': 10, 'btc': 11}, fee=0.5)
        exchange.sell(self.czk_btc(), 1, 2)
        self.assertEqual(exchange.balance('czk'), 11)
        self.assertEqual(exchange.balance('btc'), 10)

        expected = [Transaction.sell(self.czk_btc(), 0, 1, 2, 1)]
        self.assert_transactions(exchange.transactions, expected)

    def test_invalid_sell(self):
        exchange = BackTestExchange(self.TestTimeServer(), wallet={'czk': 10, 'btc': 11}, fee=0.5)
        self.assertRaises(ValueError, exchange.sell, self.czk_btc(), 11.1, 1)
        self.assertEqual(exchange.balance('czk'), 10)
        self.assertEqual(exchange.balance('btc'), 11)

    def test_balance(self):
        exchange = BackTestExchange(self.TestTimeServer, wallet={'czk': 10., 'btc': 11.})
        self.assertEqual(exchange.balance('btc'), 11.)
