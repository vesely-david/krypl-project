import datetime as dt
from matplotlib.finance import candlestick2_ohlc
import matplotlib.pyplot as plt
from matplotlib.ticker import MaxNLocator, FuncFormatter
import datetime as datetime


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

    fig, ax = plt.subplots(figsize=(9, 6))
    candlestick2_ohlc(ax, ohlc['open'], ohlc['high'], ohlc['low'], ohlc['close'], width=0.6)

    xdate = [datetime.datetime.fromtimestamp(i) for i in ohlc['timestamp']]
    ax.xaxis.set_major_locator(MaxNLocator(6))
    ax.xaxis.set_major_formatter(FuncFormatter(getDate))

    fig.autofmt_xdate()
    fig.tight_layout()
    plt.show()