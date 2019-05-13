import numpy as np
from modeling.strategy import Strategy


def supres(ltp, n):
    """
    This function takes a numpy array of last traded price
    and returns a list of support and resistance levels
    respectively. n is the number of entries to be scanned.
    """
    from scipy.signal import savgol_filter as smooth

    # converting n to a nearest even number
    if n % 2 != 0:
        n += 1
    n_half = int(n / 2)

    n_ltp = ltp.shape[0]

    # smoothening the curve
    ltp_s = smooth(ltp, n + 1, 3)
    ltp_s = np.insert(ltp_s[n + 1:], 0, [0] * (n + 1))

    # taking a simple derivative
    ltp_d = np.zeros(n_ltp)
    ltp_d[1:] = np.subtract(ltp_s[1:], ltp_s[:-1])

    resistance = []
    support = []

    for i in range(n_ltp - n):
        arr_sl = ltp_d[i:(i + n)]
        first = arr_sl[:n_half]  # first half
        last = arr_sl[n_half:]  # second half

        r_1 = np.sum(first > 0)
        r_2 = np.sum(last < 0)

        s_1 = np.sum(first < 0)
        s_2 = np.sum(last > 0)

        # local maxima detection
        if (r_1 == n_half) and (r_2 == n_half):
            k = i + (n_half - 1)
            resistance.append([k, ltp[k]])

        # local minima detection
        if (s_1 == n_half) and (s_2 == n_half):
            k = i + (n_half - 1)
            support.append([k, ltp[k]])

    return np.array(support), np.array(resistance)


class BaseMLStrategy(Strategy):
    def __init__(self, exchange, data_manager, contract_pair, willing_loss, min_profit_target, model):
        super().__init__(exchange, data_manager, contract_pair, 0, willing_loss, 0)
        self.history_len = 1
        self.min_profit_target = min_profit_target
        self.start_trade_size = self.exchange.balance(self.contract_pair['priceContract']) * 0.9
        self.trade_size = self.start_trade_size
        self.model = model

    def target_price(self, target, price_bought):
        fee_part = (1 - self.exchange.fee) ** 2
        price_part = (target + 1) * price_bought
        return price_part / fee_part

    def trade(self):
        target_i = 0
        last_support = []
        price_contract = self.contract_pair['priceContract']
        price_to_open = -1
        while self.data_manager.has_tick():
            history, price = self.data_manager.tick(self.history_len)

            if history.shape[0] < self.history_len:
                continue

            support = history[0]
            features = history[1:]
            if not self.opened and last_support != support:
                predicted_target = self.model.predict(features)
                if predicted_target <= self.min_profit_target:
                    price_to_open = support
                else:
                    price_to_open = -1

            if not self.opened and price <= price_to_open:
                self.trade_size = self.start_trade_size
                self.target_profit = predicted_target
                for i in range(2):
                    try:
                        self.buy(price)
                        break
                    except ValueError:
                        self.trade_size = self.exchange.balance(price_contract) * 0.9
                price_bought = price
                last_support = support
            elif self.opened and self.is_risky(price_bought, price):
                self.sell_all(price)
            elif self.opened and self.is_target_satisfied(price_bought, price):
                t_price = self.target_price(self.target_profit, price_bought)
                self.sell_all(t_price)

        if self.opened:
            self.sell_all(price)
