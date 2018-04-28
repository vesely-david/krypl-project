from unittest import TestCase
import pandas as pd
from trading.dataManager import CurrencyDataManager


class TestCurrencyDataManager(TestCase):

    def assert_tick(self, history, price, expected_vals, expected_price):
        for col in range(history.shape[1]):
            col_vals = history[:, col].tolist()
            self.assertEqual(col_vals, expected_vals)
        self.assertEqual(price, expected_price)

    def test_tick(self):
        vals = [1, 2, 3, 4]
        prices = pd.DataFrame({
            'timestamp': vals,
            'open': vals,
            'close': vals,
            'low': vals,
            'high': vals,
            'volume': vals
        })
        manager = CurrencyDataManager(prices['close'], prices)
        h0, p0 = manager.tick(2)
        self.assert_tick(h0, p0, [], 1)

        h1, p1 = manager.tick(2)
        self.assert_tick(h1, p1, [1], 2)

        h2, p2 = manager.tick(2)
        self.assert_tick(h2, p2, [1, 2], 3)

        h3, p3 = manager.tick(2)
        self.assert_tick(h3, p3, [2, 3], 4)
