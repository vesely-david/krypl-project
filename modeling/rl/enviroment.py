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
    return Box(low=data[col].min(), high=data[col].max(), shape=historyLen)


class ExchangeEnv(Env):

    def __init__(self, prices, historyLen, contractPair, wallet, fee, epochLen, buyAmount):
        self.startTime = 0
        self.epochLen = epochLen
        self.dataSize = prices.shape[0]
        self.dataManager = CurrencyDataManager(prices)
        self.exchange = BackTestExchange(self.dataManager, wallet, fee)
        self.initialExchange = deepcopy(self.exchange)
        self.historyLen = historyLen
        self.contractPair = contractPair
        self.buyAmount = buyAmount
        self.lastPrice = -1

        self.lastAction = HOLD
        self.action_space = Discrete(3)
        self.observation_space = Dict({
            'open': boxFromData(prices, 'open', historyLen),
            'close': boxFromData(prices, 'close', historyLen),
            'high': boxFromData(prices, 'high', historyLen),
            'low': boxFromData(prices, 'low', historyLen),
            'volume': boxFromData(prices, 'volume', historyLen),
            'price': Box(low=-np.inf, high=np.inf, shape=1),
            'amountOfPriceContract': Box(low=-np.inf, high=np.inf, shape=1),
            'amountOfTradeContract': Box(low=-np.inf, high=np.inf, shape=1),
            'lastAction': Discrete(3)
        })

    def _getObservation(self):
        history, self.lastPrice = self.dataManager.tick(self.historyLen)
        return {
            'open': history['open'],
            'close': history['close'],
            'high': history['high'],
            'low': history['low'],
            'volume': history['volume'],
            'price': self.lastPrice,
            'amountOfPriceContract': self.exchange.balance(self.contractPair.priceContract),
            'amountOfTradeContract': self.exchange.balance(self.contractPair.tradeContract),
            'lastAction': self.lastAction
        }

    def reset(self):
        self.startTime = randint(self.historyLen, self.dataSize-self.historyLen-1)
        self.dataManager.time = self.startTime
        return self._getObservation()

    def render(self, mode='human'):
        pass

    def isDone(self):
        return self.dataManager.time - self.startTime == self.epochLen

    def reward(self):
        initialBalance = self.initialExchange.balance(self.contractPair.priceContract)
        currentBalance = self.exchange.balance(self.contractPair.priceContract)
        return (currentBalance / initialBalance) - 1

    def step(self, action):
        if action == HOLD:
            return self._getObservation(), 0.0, self.isDone(), {}
        elif action == BUY:
            self.exchange.buy(self.contractPair, self.buyAmount, self.lastPrice)
            return self._getObservation(), 0.0, self.isDone(), {}
        elif action == SELL:
            amount = self.exchange.balance(self.contractPair.tradeContract)
            self.exchange.sell(self.contractPair, amount, self.lastPrice)
            return self._getObservation(), self.reward(), self.isDone(), {}
