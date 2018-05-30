from copy import deepcopy
from trading.statistics import Statistics


class Strategy:
    def __init__(self, exchange, data_manager, contract_pair, trade_size, willing_loss, target_profit):
        self.exchange = exchange
        self.wallet_start = deepcopy(self.exchange.wallet)
        self.data_manager = data_manager
        self.contract_pair = contract_pair
        self.opened = False
        self.trade_size = trade_size
        self.willing_loss = willing_loss
        self.target_profit = target_profit

    def is_risky(self, price_bought, actual_price):
        return self.current_return(price_bought, actual_price) < -self.willing_loss

    def current_return(self, price_bought, actual_price):
        fee_part = (1 - self.exchange.fee) ** 2
        return_part = (actual_price * fee_part) / price_bought
        return return_part - 1

    def sell_all(self, price):
        self.opened = False
        amount = self.exchange.balance(self.contract_pair['tradeContract'])
        self.exchange.sell(self.contract_pair, amount, price)

    def buy(self, price):
        amount = self.trade_size / price
        self.exchange.buy(self.contract_pair, amount, price)
        self.opened = True

        fee = self.exchange.fee
        break_even_price = price / ((1 - fee) ** 2)
        return break_even_price

    def is_target_satisfied(self, price_bought, price):
        return self.current_return(price_bought, price) >= self.target_profit

    def current_time(self):
        return self.data_manager.time

    def time_exceeded(self, time_bought):
        return (self.current_time() - time_bought) > self.history_len

    def stats(self, contract_name):
        return Statistics(contract_name, self.wallet_start[contract_name]).evaluate(self.exchange.transactions)

    def trade(self):
        raise NotImplementedError()
