from unittest import TestCase
import pandas as pd
from trading.dataManager import CurrencyDataManager


class TestCurrencyDataManager(TestCase):

    def assertTick(self, history, price, expectedVals, expectedPrice):
        for col in ['open', 'close', 'low', 'high', 'volume']:
            colVals = history[col].values.tolist()
            self.assertEqual(colVals, expectedVals)
        self.assertEqual(price, expectedPrice)

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
        manager = CurrencyDataManager(prices)
        h1, p1 = manager.tick(2)
        self.assertTick(h1, p1, [1], 2)

        h2, p2 = manager.tick(2)
        self.assertTick(h2, p2, [1, 2], 3)

        h3, p3 = manager.tick(2)
        self.assertTick(h3, p3, [2, 3], 4)
