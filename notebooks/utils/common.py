import datetime as dt
import pickle
import numpy as np
import matplotlib.pyplot as plt
from matplotlib.finance import candlestick2_ohlc
from matplotlib.ticker import MaxNLocator, FuncFormatter
from trading.money.transaction import BuyTransaction, SellTransaction


def timestampToDate(timestamp):
    return dt.datetime.fromtimestamp(int(timestamp))


def strTimeToTimestamp(strTime):
    return dt.datetime.strptime(strTime, "%Y-%m-%d %H:%M:%S").timestamp()


def plotCandles(ohlc):
    def getDate(x, _):
        try:
            return xdate[int(x)]
        except IndexError:
            return ''

    fig, ax = plt.subplots(figsize=(9, 4))
    candlestick2_ohlc(ax, ohlc['open'], ohlc['high'], ohlc['low'], ohlc['close'], width=0.6)

    xdate = [dt.datetime.fromtimestamp(i) for i in ohlc['timestamp']]
    ax.xaxis.set_major_locator(MaxNLocator(6))
    ax.xaxis.set_major_formatter(FuncFormatter(getDate))

    fig.autofmt_xdate()
    fig.tight_layout()
    plt.show()


def transactionsToPlot(transactions, ttype):
    filtered = [[t.timestamp, t.price.value] for t in transactions if type(t) == ttype]
    return np.array(filtered)


def plotTransactions(ohlc, transactions):
    plotCandles(ohlc)
    buys = transactionsToPlot(transactions, BuyTransaction)
    sells = transactionsToPlot(transactions, SellTransaction)

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
