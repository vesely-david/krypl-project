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

    def __init__(self, data, priceCol, contractPair, wallet, fee, epochLen, buyAmount):
        self.startTime = 0
        self.epochLen = epochLen
        self.dataSize = data.shape[0]

        featureCols = [c for c in data.columns if c != priceCol]
        self.dataManager = CurrencyDataManager(data[priceCol], data[featureCols])
        self.exchange = BackTestExchange(self.dataManager, deepcopy(wallet), fee)
        self.initialExchange = deepcopy(self.exchange)
        self.contractPair = contractPair
        self.buyAmount = buyAmount
        self.lastAction = HOLD
        self.priceCol = priceCol

        self.action_space = Discrete(3)
        self.observationClass = Observation
        self.observation_space = self.observationClass.observationSpace(data.shape[1]-1)
        self.metadata = {'render.modes': ['human']}
        self.lastObservation = None
        self.lastPrice = -1

    def _getObservation(self):
        history, price = self.dataManager.tick(1)
        self.lastObservation = history
        self.lastPrice = price
        return self.lastObservation

    def reset(self):
        self.startTime = randint(1, self.dataSize-self.epochLen-1)
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
        priceContract = self.contractPair['priceContract']
        initialBalance = self.initialExchange.balance(priceContract)
        currentBalance = self.exchange.balance(priceContract)
        return (currentBalance / initialBalance) - 1

    def step(self, action):
        self.lastAction = action
        if action == HOLD:
            return self._getObservation(), 0.0, self._isDone(), {}
        elif action == BUY:
            try:
                self.exchange.buy(self.contractPair, self.buyAmount, self.lastPrice)
            finally:
                return self._getObservation(), 0.0, self._isDone(), {}
        elif action == SELL:
            try:
                amount = self.exchange.balance(self.contractPair['tradeContract'])
                self.exchange.sell(self.contractPair, amount, self.lastPrice)
                return self._getObservation(), self._reward(), self._isDone(), {}
            except ValueError:
                return self._getObservation(), 0.0, self._isDone(), {}


class Observation(object):

    def __init__(self, rowDf, excludeCols=[]):
        cols = [c for c in rowDf.columns if c not in excludeCols]
        self.featureRow = rowDf[cols].values[0]

    def get(self):
        return self.featureRow

    @staticmethod
    def observationSpace(numOfFeatures):
        space = Box(low=-np.inf, high=np.inf, shape=(numOfFeatures,), dtype=np.float32)
        space.n = numOfFeatures
        return space

