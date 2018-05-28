import pandas as pd
import datetime as dt
import pickle
import numpy as np
import matplotlib.pyplot as plt
from mpl_finance import candlestick2_ohlc
from matplotlib.ticker import MaxNLocator, FuncFormatter
from trading.money.transaction import Transaction


def timestamp_to_date(timestamp):
    return dt.datetime.fromtimestamp(int(timestamp))


def str_time_to_datetime(str_time):
    return dt.datetime.strptime(str_time, "%Y-%m-%d %H:%M:%S")


def str_time_to_timestamp(str_time):
    return str_time_to_datetime(str_time).timestamp()


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


def plot_transactions(ohlc, transactions):
    plot_candles(ohlc)
    buys = transactions_to_plot(transactions, Transaction.BUY)
    sells = transactions_to_plot(transactions, Transaction.SELL)

    buyScatter = plt.scatter(buys[:, 0], buys[:, 1], s=50, c='g', label='buy')
    sellScatter = plt.scatter(sells[:, 0], sells[:, 1], s=50, c='m', label='sell')

    plt.legend(handles=[buyScatter, sellScatter], loc='upper left')


def load_data(file_name):
    with open(file_name, "rb") as f:
        data = pickle.load(f)
    return data


def save_data(data, file_name):
    with open(file_name, "wb") as f:
        pickle.dump(data, f)


def read_tsv(file):
    return pd.read_csv(file, sep='\t')


def write_tsv(df, file):
    df.to_csv(file, index=False, sep='\t')
