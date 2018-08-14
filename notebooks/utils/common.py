import pandas as pd
import datetime as dt
import pickle
import numpy as np
import matplotlib.pyplot as plt
from mpl_finance import candlestick2_ohlc
from matplotlib.ticker import MaxNLocator, FuncFormatter
from trading.money.transaction import Transaction
from sqlite3 import connect


def resolve_multiple(args, f):
    if len(args) == 0:
        return None
    elif len(args) == 1:
        return f(args[0])
    else:
        return [f(arg) for arg in args]


def timestamp_to_date(*timestamps):
    def f(x):
        return dt.datetime.fromtimestamp(int(x))
    return resolve_multiple(timestamps, f)


def str_time_to_datetime(*str_times):
    def f(x):
        return dt.datetime.strptime(x, "%Y-%m-%d %H:%M:%S")
    return resolve_multiple(str_times, f)


def str_time_to_timestamp(*str_times):
    def f(x):
        return str_time_to_datetime(x).timestamp()
    return resolve_multiple(str_times, f)


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


def read_tsv(file, header=0):
    return pd.read_csv(file, sep='\t', header=header)


def write_tsv(df, file):
    df.to_csv(file, index=False, sep='\t')


def read_table(table, db=None, conn=None):
    if conn is None:
        conn = connect(db)
    return pd.read_sql(f"select * from {table}", conn)


def load_trading_data(db, table, from_date=None, to_date=None, period=None):
    data = read_table(table, db) \
        .sort_values('date').reset_index().drop('index', axis=1)
    data['timestamp'] = data.date.apply(str_time_to_timestamp).astype(int)
    if from_date is not None:
        data = data.query(f"date >= '{from_date}'")
    if to_date is not None:
        data = data.query(f"date < '{to_date}'")
    if period is not None:
        data = data.query(f"period == '{period}'")

    return data


def divide_train_and_test(data, train_ratio=0.7):
    train_size = int(data.shape[0] * train_ratio)
    data_train = data.iloc[:train_size]
    data_test = data.iloc[train_size:].reset_index().drop('index', axis=1)
    return data_train, data_test


def load_split(root):
    X_train = read_tsv(f"{root}/X_train.tsv")
    y_train = read_tsv(f"{root}/y_train.tsv", header=None).iloc[:, 0]
    X_val = read_tsv(f"{root}/X_val.tsv")
    y_val = read_tsv(f"{root}/y_val.tsv", header=None).iloc[:, 0]
    X_test = read_tsv(f"{root}/X_test.tsv")
    y_test = read_tsv(f"{root}/y_test.tsv", header=None).iloc[:, 0]
    return X_train, y_train, X_val, y_val, X_test, y_test
