from unittest import TestCase
from trading.money.transaction import BuyTransaction, SellTransaction
from trading.money.contract import ContractPair
from trading.statistics import Statistics


class TestStatistics(TestCase):
    def setUp(self):
        self.cPair = ContractPair('czk', 'btc')
        self.statsCzk = Statistics('czk', startAmount=2000)

        self.scenarioWinLong = [
            BuyTransaction(self.cPair, timestamp=0, amount=10, price=100, fee=10),
            SellTransaction(self.cPair, timestamp=1, amount=10, price=200, fee=10),
        ]

        self.scenarioWinShort = [
            SellTransaction(self.cPair, timestamp=0, amount=10, price=200, fee=10),
            BuyTransaction(self.cPair, timestamp=1, amount=10, price=100, fee=10),
        ]

        self.scenarioLossLong = [
            BuyTransaction(self.cPair, timestamp=0, amount=10, price=200, fee=10),
            SellTransaction(self.cPair, timestamp=1, amount=10, price=100, fee=10)
        ]

        self.scenarioLossShort = [
            SellTransaction(self.cPair, timestamp=0, amount=10, price=100, fee=10),
            BuyTransaction(self.cPair, timestamp=1, amount=10, price=200, fee=10),
        ]

    def assertStats(self, stats, valuesDict):
        self.assertEqual(stats.numberOfTrades, valuesDict['numberOfTrades'])
        self.assertEqual(stats.totalProfit, valuesDict['totalProfit'])
        self.assertEqual(stats.avgProfit, valuesDict['avgProfit'])
        self.assertEqual(stats.winPercentage, valuesDict['winPercentage'])
        self.assertEqual(stats.avgWinTrade, valuesDict['avgWinTrade'])
        self.assertEqual(stats.avgLossTrade, valuesDict['avgLossTrade'])
        self.assertAlmostEqual(stats.profitFactor, valuesDict['profitFactor'], places=2)
        self.assertAlmostEqual(stats.maxDrawdown, valuesDict['maxDrawdown'], places=3)

    def test_evaluatePrimaryContractLongWin(self):
        scenario = self.scenarioWinLong + self.scenarioWinLong
        stats = self.statsCzk.evaluate(scenario)
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
        self.assertStats(stats, results)

    def test_evaluatePrimaryContractLongLoss(self):
        scenario = self.scenarioLossLong + self.scenarioLossLong
        stats = self.statsCzk.evaluate(scenario)
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
        self.assertStats(stats, results)

    def test_evaluatePrimaryContractShortWin(self):
        scenario = self.scenarioWinShort + self.scenarioWinShort
        stats = self.statsCzk.evaluate(scenario)
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
        self.assertStats(stats, results)

    def test_evaluatePrimaryContractShortLoss(self):
        scenario = self.scenarioLossShort + self.scenarioLossShort
        stats = self.statsCzk.evaluate(scenario)
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
        self.assertStats(stats, results)

    def test_maxDrawdown(self):
        scenario = self.scenarioWinLong + self.scenarioLossLong + self.scenarioWinLong + \
                   self.scenarioWinLong + self.scenarioLossLong

        stats = self.statsCzk.evaluate(scenario)
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
        self.assertStats(stats, results)
