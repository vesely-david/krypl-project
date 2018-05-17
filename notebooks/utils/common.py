import pandas as pd
import datetime as dt
import pickle
import numpy as np
import matplotlib.pyplot as plt
from matplotlib.finance import candlestick2_ohlc
from matplotlib.ticker import MaxNLocator, FuncFormatter
from trading.money.transaction import Transaction


def timestampToDate(timestamp):
    return dt.datetime.fromtimestamp(int(timestamp))


def strTimeToDatetime(strTime):
    return dt.datetime.strptime(strTime, "%Y-%m-%d %H:%M:%S")


def strTimeToTimestamp(strTime):
    return strTimeToDatetime(strTime).timestamp()


def set_date_axis(timestamp_data, ax, fig):
    def getDate(x, _):
        try:
            return xdate[int(x)]
        except IndexError:
            return ''

    xdate = [dt.datetime.fromtimestamp(i) for i in timestamp_data]
    ax.xaxis.set_major_locator(MaxNLocator(6))
    ax.xaxis.set_major_formatter(FuncFormatter(getDate))

    fig.autofmt_xdate()
    fig.tight_layout()


def plotCandles(ohlc):
    fig, ax = plt.subplots(figsize=(12, 6))
    candlestick2_ohlc(ax, ohlc['open'], ohlc['high'], ohlc['low'], ohlc['close'], width=0.6)
    set_date_axis(ohlc['timestamp'], ax, fig)
    plt.show()
    return fig, ax


def transactionsToPlot(transactions, _type):
    filtered = [[t['timestamp'], t['price']] for t in transactions if t['type'] == _type]
    return np.array(filtered)


def plotTransactions(ohlc, transactions):
    plotCandles(ohlc)
    buys = transactionsToPlot(transactions, Transaction.BUY)
    sells = transactionsToPlot(transactions, Transaction.SELL)

    buyScatter = plt.scatter(buys[:, 0], buys[:, 1], s=20, c='g', label='buy')
    sellScatter = plt.scatter(sells[:, 0], sells[:, 1], s=20, c='m', label='sell')

    plt.legend(handles=[buyScatter, sellScatter], loc='upper left')


def loadData(fileName):
    with open(fileName, "rb") as f:
        data = pickle.load(f)
    return data


def saveData(data, fileName):
    with open(fileName, "wb") as f:
        pickle.dump(data, f)


def readTsv(file):
    return pd.read_csv(file, sep='\t')


def writeTsv(df, file):
    df.to_csv(file, index=False, sep='\t')
