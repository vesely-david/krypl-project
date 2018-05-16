import numpy as np
from random import randint

from gym import Env
from gym.spaces.discrete import Discrete
from gym.spaces.box import Box
from trading.dataManager import CurrencyDataManager
from trading.exchange import BackTestExchange
from copy import deepcopy

HOLD = 0
BUY = 1
SELL = 2


def box_from_data(data, col, history_len):
    return Box(low=data[col].min(), high=data[col].max(), shape=(history_len,), dtype=np.float32)


class ExchangeEnv(Env):

    def __init__(self, data, price_col, pair, wallet, fee, epoch_len, trade_amount):
        self.start_time = 0
        self.epoch_len = epoch_len
        self.data_size = data.shape[0]

        features = [c for c in data.columns if c != price_col]
        self.data_manager = CurrencyDataManager(data[price_col], data[features])
        self.exchange = BackTestExchange(self.data_manager, deepcopy(wallet), fee)
        self.initial_wallet = deepcopy(wallet)
        self.pair = pair
        self.trade_amount = trade_amount
        self.last_action = HOLD
        self.price_col = price_col

        self.action_space = Discrete(3)
        self.observation_class = Observation
        self.observation_space = self.observation_class.observation_space(len(features))
        self.metadata = {'render.modes': ['human']}
        self.last_observation = None
        self.last_price = -1
        self.open = False

    def _get_observation(self):
        history, price = self.data_manager.tick(1)
        self.last_observation = history
        self.last_price = price
        return self.last_observation

    def reset(self):
        self.start_time = randint(1, self.data_size - self.epoch_len - 1)
        self.data_manager.time = self.start_time
        self.exchange.wallet = deepcopy(self.initial_wallet)
        return self._get_observation()

    def render(self, mode='human'):
        if mode == 'human':
            pass
        else:
            super(ExchangeEnv, self).render(mode=mode)

    def _is_done(self):
        return self.data_manager.time - self.start_time == self.epoch_len

    def _portfolio_value(self, wallet):
        price_contract = self.pair['priceContract']
        trade_contract = self.pair['tradeContract']
        price_value = wallet.get(price_contract, 0.)
        trade_value = wallet.get(trade_contract, 0.) * self.last_price
        return price_value + trade_value

    def _reward(self):
        return (self._portfolio_value(self.exchange.wallet) / self._portfolio_value(self.initial_wallet)) - 1

    def _buy_amount(self):
        return self.trade_amount / self.last_price

    def step(self, action):
        self.last_action = action
        err = False
        try:
            if action == BUY:
                self.exchange.buy(self.pair, self._buy_amount(), self.last_price)
                self.open = True
            elif action == SELL and self.open:
                amount = self.exchange.balance(self.pair['tradeContract'])
                self.exchange.sell(self.pair, amount, self.last_price)
                self.open = False
        except ValueError:
            err = True

        debug = {
            'current_value': self._portfolio_value(self.exchange.wallet),
            'initial_value': self._portfolio_value(self.initial_wallet),
            'err': err,
            'open': self.open
        }
        return self._get_observation(), self._reward(), self._is_done(), debug


class Observation(object):

    @staticmethod
    def observation_space(numOfFeatures):
        space = Box(low=-np.inf, high=np.inf, shape=(numOfFeatures,), dtype=np.float32)
        space.n = numOfFeatures
        return space

