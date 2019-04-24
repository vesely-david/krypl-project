import numpy as np


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


from modeling.strategy import Strategy


class BaseStrategy(Strategy):
    def __init__(self, exchange, data_manager, contract_pair, willing_loss, target_profit, target_return):
        super().__init__(exchange, data_manager, contract_pair, 0, willing_loss, target_profit)
        self.history_len = 2
        self.target_return = target_return
        
    
    def target_price(self, price_bought):
        fee_part = (1 - self.exchange.fee) ** 2
        price_part = (self.target_profit + 1) * price_bought
        return price_part / fee_part
    
    
    def trade(self):
        hold = 0
        last_return = 0
        price_contract = self.contract_pair['priceContract']
        while self.data_manager.has_tick():
            history, price = self.data_manager.tick(self.history_len)

            if history.shape[0] < 2:
                continue
            
            sup_f, sup_s = history[:, 0]
            _return = (sup_s / sup_f) - 1
            if not self.opened and _return != last_return and _return <= self.target_return:
                try:
                    self.trade_size = self.exchange.balance(price_contract) * 0.98
                    self.buy(price)
                    price_bought = price
                    hold = 0
                    last_return =_return
                except ValueError:
                    break
                    
            elif self.opened and self.is_risky(price_bought, price):
                self.sell_all(price)
                hold = 0
            elif self.opened and self.is_target_satisfied(price_bought, price):
                self.sell_all(self.target_price(price_bought))
                hold = 0

            if self.opened:
                hold += 1
            

        if self.opened:
            self.sell_all(price)      
