import pandas as pd
import numpy as np
from trading.money.transaction import Transaction


class Statistics:
    def __init__(self, contract_name, start_amount):
        self.contract_name = contract_name
        self.transactions = []
        self.open_transactions = []
        self.close_transactions = []
        self.start_amount = start_amount

        self.current_amount = start_amount
        self.last_amount = start_amount
        self.max_amount = start_amount

        self.num_of_trades = 0
        self.num_of_wins = 0
        self.total_won = 0
        self.total_loss = 0
        self.current_loss = 0
        self.max_drawdown_var = .0

    def report(self):
        cols = ['startAmount', 'numberOfTrades', 'totalProfit', 'avgProfit', 'winPercentage', 'avgWinTrade',
                'avgLossTrade', 'profitFactor', 'maxDrawdown', 'avgTimeToClose', 'minTimeToClose', 'maxTimeToClose']
        stats = pd.DataFrame({
            'startAmount': [self.start_amount],
            'numberOfTrades': [self.number_of_trades()],
            'totalProfit': [self.total_profit()],
            'avgProfit': [self.avg_profit()],
            'winPercentage': [self.win_percentage()],
            'avgWinTrade': [self.avg_win_trade()],
            'avgLossTrade': [self.avg_loss_trade()],
            'profitFactor': [self.profit_factor()],
            'maxDrawdown': [self.max_drawdown()],
            'avgTimeToClose': [self.avg_time_to_close()],
            'minTimeToClose': [self.min_time_to_close()],
            'maxTimeToClose': [self.max_time_to_close()]
        })[cols].transpose()
        stats.columns = [self.contract_name]
        return stats

    def evaluate(self, transactions):
        for transaction in transactions:
            self.add_transaction(transaction)
        return self

    def add_transaction(self, transaction):
        if self._is_next_transaction_open():
            self.open_trade(transaction)
        else:
            self.close_trade(transaction)

    def open_trade(self, transaction):
        self.transactions.append(transaction)
        self.open_transactions.append(transaction)
        self.last_amount = self.current_amount
        self.current_amount += self._contract_amount(transaction) - self._fee(transaction)

    def close_trade(self, transaction):
        self.transactions.append(transaction)
        self.close_transactions.append(transaction)
        self.current_amount += self._contract_amount(transaction) - self._fee(transaction)
        self.num_of_trades += 1
        self._update_max_amount()
        self._update_wins()
        self._update_losses()

    def number_of_trades(self):
        return self.num_of_trades

    def total_profit(self):
        return self.current_amount - self.start_amount

    def avg_profit(self):
        return float(self.total_profit()) / self.number_of_trades() if self.number_of_trades() > 0 else 0

    def win_percentage(self):
        return 100. * self.num_of_wins / self.number_of_trades() if self.number_of_trades() > 0 else 0

    def avg_win_trade(self):
        return float(self.total_won) / self.num_of_wins if self.num_of_wins > 0 else 0

    def avg_loss_trade(self):
        num_of_losses = self.number_of_trades() - self.num_of_wins
        return float(-self.total_loss) / num_of_losses if num_of_losses > 0 else 0

    def profit_factor(self):
        return float(self.total_won) / self.total_loss if self.total_loss > 0 else float('inf')

    def max_drawdown(self):
        return self.max_drawdown_var

    def min_time_to_close(self):
        if self.number_of_trades() == 0:
            return np.nan
        diffs = self._get_time_diffs()
        return min(diffs)

    def max_time_to_close(self):
        if self.number_of_trades() == 0:
            return np.nan
        diffs = self._get_time_diffs()
        return max(diffs)

    def avg_time_to_close(self):
        if self.number_of_trades() == 0:
            return np.nan
        diffs = self._get_time_diffs()
        return np.mean(diffs)

    # ------------ Private --------------------------
    def _get_time_diffs(self):
        open_times = np.array([t['timestamp'] for t in  self.open_transactions])
        close_times = np.array([t['timestamp'] for t in  self.close_transactions])
        return close_times - open_times

    def _fee(self, transaction):
        pair = transaction['pair']
        if pair['priceContract'] == self.contract_name:
            return transaction['fee']
        else:
            return transaction['fee'] / transaction['price']

    def _update_max_amount(self):
        if self.current_amount > self.max_amount:
            self.max_amount = self.current_amount

    def _update_wins(self):
        if self._is_last_win():
            self.num_of_wins += 1
            self.total_won += self.current_amount - self.last_amount

    def _update_drawdown(self):
        drawdown = 100. * (self.max_amount - self.current_amount) / self.max_amount
        if drawdown > self.max_drawdown_var:
            self.max_drawdown_var = drawdown

    def _update_losses(self):
        if self._is_last_loss():
            self.total_loss += self.last_amount - self.current_amount
            self._update_drawdown()

    def _num_of_transactions(self):
        return len(self.transactions)

    def _is_next_transaction_open(self):
        return self._num_of_transactions() % 2 == 0

    def _is_next_transaction_close(self):
        return not self._is_next_transaction_open()

    def _is_last_win(self):
        return self.current_amount > self.last_amount

    def _is_last_loss(self):
        return not self._is_last_win()

    def _contract_amount(self, transaction):
        gained_name, gained_value = Transaction.gained_contract(transaction)
        subtracted_name, subtracted_value = Transaction.subtracted_contract(transaction)
        if gained_name == self.contract_name:
            return gained_value
        elif subtracted_name == self.contract_name:
            return -subtracted_value

        raise ValueError('Transaction does not consist wanted contract.')
