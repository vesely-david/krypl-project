import numpy as np
from random import randint

from gym import Env
from gym.spaces.discrete import Discrete
from gym.spaces.dict_space import Dict
from gym.spaces.box import Box
from trading.dataManager import CurrencyDataManager
from trading.exchange import BackTestExchange

HOLD = 0
BUY = 1
SELL = 2


def boxFromData(data, col, historyLen):
    return Box(low=data[col].min(), high=data[col].max(), shape=historyLen)


class ExchangeEnv(Env):

    def __init__(self, prices, historyLen, contractPair, wallet, fee):
        self.dataSize = prices.shape[0]
        self.dataManager = CurrencyDataManager(prices)
        self.exchange = BackTestExchange(self.dataManager, wallet, fee)
        self.historyLen = historyLen
        self.action_space = Discrete(3)
        self.priceContract = contractPair.priceContract
        self.tradeContract = contractPair.tradeContract
        self.observation_space = Dict({
            'open': boxFromData(prices, 'open', historyLen),
            'close': boxFromData(prices, 'close', historyLen),
            'high': boxFromData(prices, 'high', historyLen),
            'low': boxFromData(prices, 'low', historyLen),
            'volume': boxFromData(prices, 'volume', historyLen),
            'price': Box(low=-np.inf, high=np.inf, shape=1),
            'amountOfPriceContract': Box(low=-np.inf, high=np.inf, shape=1),
            'amountOfTradeContract': Box(low=-np.inf, high=np.inf, shape=1)
        })

    def _getObservation(self, i):
        self.dataManager.time = i
        history, price = self.dataManager.tick(self.historyLen)
        return {
            'open': history['open'],
            'close': history['close'],
            'high': history['high'],
            'low': history['low'],
            'volume': history['volume'],
            'price': price,
            'amountOfPriceContract': self.exchange.balance(self.priceContract),
            'amountOfTradeContract': self.exchange.balance(self.tradeContract),
        }

    def reset(self):
        i = randint(self.historyLen, self.dataSize-self.historyLen-1)
        return self._getObservation(i)

    def render(self, mode='human'):
        pass

    def step(self, action):
        pass





