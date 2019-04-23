import os
from utils.common import load_trading_data


def read_data(pair, _dir, from_date=None, to_date=None):
    db = os.path.join(_dir, 'sqlite', 'ploniex-chart-data', f'{pair}.db')
    data = load_trading_data(db, 'chart_data', from_date=from_date, to_date=to_date, period='30min')
    data['ohlc4'] = (data['open'] + data['close'] + data['high'] + data['low']) / 4
    return data


def read_train(pair, _dir):
    return read_data(pair, _dir, '2016-01-01', '2018-01-01')


def read_test(pair, _dir):
    return read_data(pair, _dir, '2018-01-01', '2019-04-01')
