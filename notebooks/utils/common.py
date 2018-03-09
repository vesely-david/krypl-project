import time
import datetime as dt


def timestampToDate(timestamp):
    return dt.datetime.fromtimestamp(int(timestamp))


def strTimeToTimestamp(strTime):
    return dt.datetime.strptime(strTime, "%Y-%m-%d %H:%M:%S").timestamp()
