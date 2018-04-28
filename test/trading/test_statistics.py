from unittest import TestCase
from trading.money.transaction import Transaction
from trading.money.contract import ContractPair
from trading.statistics import Statistics


class TestStatistics(TestCase):
    def setUp(self):
        self.pair = ContractPair.new('czk', 'btc')
        self.stats_czk = Statistics('czk', start_amount=2000)

        self.scenario_win_long = [
            Transaction.buy(self.pair, timestamp=0, amount=10, price=100, fee=10),
            Transaction.sell(self.pair, timestamp=1, amount=10, price=200, fee=10),
        ]

        self.scenario_win_short = [
            Transaction.sell(self.pair, timestamp=0, amount=10, price=200, fee=10),
            Transaction.buy(self.pair, timestamp=1, amount=10, price=100, fee=10),
        ]

        self.scenario_loss_long = [
            Transaction.buy(self.pair, timestamp=0, amount=10, price=200, fee=10),
            Transaction.sell(self.pair, timestamp=1, amount=10, price=100, fee=10)
        ]

        self.scenario_loss_short = [
            Transaction.sell(self.pair, timestamp=0, amount=10, price=100, fee=10),
            Transaction.buy(self.pair, timestamp=1, amount=10, price=200, fee=10),
        ]

    def assert_stats(self, stats, valuesDict):
        self.assertEqual(stats.number_of_trades(), valuesDict['numberOfTrades'])
        self.assertEqual(stats.total_profit(), valuesDict['totalProfit'])
        self.assertEqual(stats.avg_profit(), valuesDict['avgProfit'])
        self.assertEqual(stats.win_percentage(), valuesDict['winPercentage'])
        self.assertEqual(stats.avg_win_trade(), valuesDict['avgWinTrade'])
        self.assertEqual(stats.avg_loss_trade(), valuesDict['avgLossTrade'])
        self.assertAlmostEqual(stats.profit_factor(), valuesDict['profitFactor'], places=2)
        self.assertAlmostEqual(stats.max_drawdown(), valuesDict['maxDrawdown'], places=3)

    def test_evaluate_primary_contract_long_win(self):
        scenario = self.scenario_win_long + self.scenario_win_long
        stats = self.stats_czk.evaluate(scenario)
        results = {
            'numberOfTrades': 2,
            'totalProfit': 1960,
            'avgProfit': 980,
            'winPercentage': 100,
            'avgWinTrade': 980,
            'avgLossTrade': 0,
            'profitFactor': float('inf'),
            'maxDrawdown': 0,
        }
        self.assert_stats(stats, results)

    def test_evaluate_primary_contract_long_loss(self):
        scenario = self.scenario_loss_long + self.scenario_loss_long
        stats = self.stats_czk.evaluate(scenario)
        results = {
            'numberOfTrades': 2,
            'totalProfit': -2040,
            'avgProfit': -1020,
            'winPercentage': 0,
            'avgWinTrade': 0,
            'avgLossTrade': -1020,
            'profitFactor': 0,
            'maxDrawdown': 102,
        }
        self.assert_stats(stats, results)

    def test_evaluate_primary_contract_short_win(self):
        scenario = self.scenario_win_short + self.scenario_win_short
        stats = self.stats_czk.evaluate(scenario)
        results = {
            'numberOfTrades': 2,
            'totalProfit': 1960,
            'avgProfit': 980,
            'winPercentage': 100,
            'avgWinTrade': 980,
            'avgLossTrade': 0,
            'profitFactor': float('inf'),
            'maxDrawdown': 0,
        }
        self.assert_stats(stats, results)

    def test_evaluate_primary_contract_short_loss(self):
        scenario = self.scenario_loss_short + self.scenario_loss_short
        stats = self.stats_czk.evaluate(scenario)
        results = {
            'numberOfTrades': 2,
            'totalProfit': -2040,
            'avgProfit': -1020,
            'winPercentage': 0,
            'avgWinTrade': 0,
            'avgLossTrade': -1020,
            'profitFactor': 0,
            'maxDrawdown': 102,
        }
        self.assert_stats(stats, results)

    def test_max_drawdown(self):
        scenario = self.scenario_win_long + self.scenario_loss_long + self.scenario_win_long + \
                   self.scenario_win_long + self.scenario_loss_long

        stats = self.stats_czk.evaluate(scenario)
        results = {
            'numberOfTrades': 5,
            'totalProfit': 900,
            'avgProfit': 180,
            'winPercentage': 60,
            'avgWinTrade': 980,
            'avgLossTrade': -1020,
            'profitFactor': 1.44,
            'maxDrawdown': 34.228,
        }
        self.assert_stats(stats, results)
