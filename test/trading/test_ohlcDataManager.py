from unittest import TestCase
import pandas as pd
from trading.dataManager import OhlcDataManager


class TestOhlcDataManager(TestCase):

    def test_tick(self):
        vals = list(range(1, 9))
        prices = pd.DataFrame({
            'timestamp': vals,
            'open': vals,
            'close': [i*1000 for i in vals],
            'low': [i*10 for i in vals],
            'high': [i*100 for i in vals],
            'volume': vals
        })
        manager = OhlcDataManager(prices[['close', 'open', 'high', 'low']], prices[['volume']])

        expected_prices = []
        for i in vals:
            for j in [1, 10, 100, 1000]:
                expected_prices.append(i*j)

        a = [[], [[1]], [[1], [2]], [[2], [3]], [[3], [4]], [[4], [5]], [[5], [6]], [[6], [7]], [[7], [8]]]
        expected_histories = []
        for x in a:
            for _ in range(4):
                expected_histories.append(x)
        expected_histories

        expected_times = [round(i*0.25, 2) for i in range(len(vals) * 4)]
        for i in range(len(vals) * 4):
            self.assertEqual(expected_times[i], manager.time)
            h0, p0 = manager.tick(2)
            self.assertEqual(expected_prices[i], p0)
            self.assertEqual(expected_histories[i], h0.tolist())



