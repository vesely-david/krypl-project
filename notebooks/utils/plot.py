import datetime as dt

import matplotlib.pyplot as plt
import numpy as np
from matplotlib.ticker import MaxNLocator, FuncFormatter
from mpl_finance import candlestick2_ohlc

from trading.money.transaction import Transaction


def set_date_axis(timestamp_data, ax, fig):
    def get_date(x, _):
        try:
            return xdate[int(x)]
        except IndexError:
            return ''

    xdate = [dt.datetime.fromtimestamp(i) for i in timestamp_data]
    ax.xaxis.set_major_locator(MaxNLocator(6))
    ax.xaxis.set_major_formatter(FuncFormatter(get_date))

    fig.autofmt_xdate()
    fig.tight_layout()


def plot_candles(ohlc):
    fig, ax = plt.subplots(figsize=(10, 6))
    candlestick2_ohlc(ax, ohlc['open'], ohlc['high'], ohlc['low'], ohlc['close'], width=0.6)
    set_date_axis(ohlc['timestamp'], ax, fig)
    plt.show()
    return fig, ax


def transactions_to_plot(transactions, _type):
    filtered = [[t['timestamp'], t['price']] for t in transactions if t['type'] == _type]
    return np.array(filtered)


def restrict_ohlc(ohlc, transactions):
    timestamps = [t['timestamp'] for t in transactions]
    neighborhood = 50
    min_timestamp = abs(min(timestamps) - neighborhood)
    max_timestamp = min(max(timestamps) + neighborhood, len(ohlc))
    return ohlc.copy() \
               .iloc[min_timestamp:max_timestamp, :] \
        .reset_index()


def plot_transactions(ohlc, transactions):
    restricted = restrict_ohlc(ohlc, transactions)
    plot_candles(restricted)
    buys = transactions_to_plot(transactions, Transaction.BUY)
    sells = transactions_to_plot(transactions, Transaction.SELL)

    first_index = restricted['index'].min()
    buyScatter = plt.scatter(buys[:, 0] - first_index, buys[:, 1], s=50, c='g', label='buy')
    sellScatter = plt.scatter(sells[:, 0] - first_index, sells[:, 1], s=50, c='m', label='sell')

    plt.legend(handles=[buyScatter, sellScatter], loc='upper left')
