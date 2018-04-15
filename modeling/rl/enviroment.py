import numpy as np
from random import randint

from gym import Env
from gym.spaces.discrete import Discrete
from gym.spaces.dict_space import Dict
from gym.spaces.box import Box
from trading.dataManager import CurrencyDataManager
from trading.exchange import BackTestExchange
from copy import deepcopy

HOLD = 0
BUY = 1
SELL = 2


def boxFromData(data, col, historyLen):
    return Box(low=data[col].min(), high=data[col].max(), shape=(historyLen,), dtype=np.float32)


class ExchangeEnv(Env):

    def __init__(self, prices, historyLen, contractPair, wallet, fee, epochLen, buyAmount):
        self.startTime = 0
        self.epochLen = epochLen
        self.dataSize = prices.shape[0]
        self.dataManager = CurrencyDataManager(prices)
        self.exchange = BackTestExchange(self.dataManager, deepcopy(wallet), fee)
        self.initialExchange = deepcopy(self.exchange)
        self.historyLen = historyLen
        self.contractPair = contractPair
        self.buyAmount = buyAmount
        self.lastAction = HOLD

        self.action_space = Discrete(3)
        self.observationClass = Observation
        self.observation_space = self.observationClass.observationSpace(prices, historyLen)
        self.metadata = {'render.modes': ['human']}
        self.lastObservation = None
        self.lastPrice = -1

    def _getObservation(self):
        history, price = self.dataManager.tick(self.historyLen)
        self.lastObservation = self.observationClass(history, self.lastAction, price, self.exchange, self.contractPair)
        self.lastPrice= price
        return self.lastObservation.toDict()

    def reset(self):
        self.startTime = randint(self.historyLen, self.dataSize-self.historyLen-1)
        self.dataManager.time = self.startTime
        return self._getObservation()

    def render(self, mode='human'):
        if mode == 'human':
            pass
        else:
            super(ExchangeEnv, self).render(mode=mode)

    def _isDone(self):
        return self.dataManager.time - self.startTime == self.epochLen

    def _reward(self):
        initialBalance = self.initialExchange.balance(self.contractPair.priceContract)
        currentBalance = self.exchange.balance(self.contractPair.priceContract)
        return (currentBalance / initialBalance) - 1

    def step(self, action):
        self.lastAction = action
        if action == HOLD:
            return self._getObservation(), 0.0, self._isDone(), {}
        elif action == BUY:
            self.exchange.buy(self.contractPair, self.buyAmount, self.lastPrice)
            return self._getObservation(), 0.0, self._isDone(), {}
        elif action == SELL:
            amount = self.exchange.balance(self.contractPair.tradeContract)
            self.exchange.sell(self.contractPair, amount, self.lastPrice)
            return self._getObservation(), self._reward(), self._isDone(), {}


class Observation(object):

    def __init__(self, history, lastAction, lastPrice, exchange, contractPair):
        self.history = history
        self.lastAction = lastAction
        self.lastPrice = lastPrice
        self.exchange = exchange
        self.contractPair = contractPair

    @staticmethod
    def n(dictSpace):
        n = 0
        for s in dictSpace.spaces.values():
            if type(s) == Box:
                n += np.prod(s.shape)
            elif type(s) == Discrete:
                n += s.n
        return n

    @staticmethod
    def observationSpace(prices, historyLen):
        infBox = Box(low=-np.inf, high=np.inf, shape=(1,), dtype=np.float32)
        space = Dict({
            'open': boxFromData(prices, 'open', historyLen),
            'close': boxFromData(prices, 'close', historyLen),
            'high': boxFromData(prices, 'high', historyLen),
            'low': boxFromData(prices, 'low', historyLen),
            'volume': boxFromData(prices, 'volume', historyLen),
            'price': infBox,
            'amountOfPriceContract': infBox,
            'amountOfTradeContract': infBox,
            'lastAction': Discrete(3)
        })
        space.n = Observation.n(space)
        return space

    def toDict(self):
        return {
            'open': self.history['open'].tolist(),
            'close': self.history['close'].tolist(),
            'high': self.history['high'].tolist(),
            'low': self.history['low'].tolist(),
            'volume': self.history['volume'].tolist(),
            'price': self.lastPrice,
            'amountOfPriceContract': self.exchange.balance(self.contractPair.priceContract),
            'amountOfTradeContract': self.exchange.balance(self.contractPair.tradeContract),
            'lastAction': self.lastAction
        }