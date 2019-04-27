import os
from utils.common import load_trading_data
from multiprocessing import Pool
from tqdm import *


def read_data(pair, _dir, from_date=None, to_date=None):
    db = os.path.join(_dir, 'sqlite', 'ploniex-chart-data', f'{pair}.db')
    data = load_trading_data(db, 'chart_data', from_date=from_date, to_date=to_date, period='30min')
    data['ohlc4'] = (data['open'] + data['close'] + data['high'] + data['low']) / 4
    return data


def read_train(pair, _dir):
    return read_data(pair, _dir, '2016-01-01', '2018-01-01')


def read_test(pair, _dir):
    return read_data(pair, _dir, '2018-01-01', '2019-04-01')


def read_all(pair, _dir):
    return read_data(pair, _dir, '2016-01-01', '2019-04-01')


def run_parallel(f, args, n_process=10):
    results = []
    with Pool(processes=n_process) as p:
        if type(args) == list:
            all_args = args
        else:
            all_args = list(args)
        with tqdm_notebook(enumerate(p.imap_unordered(f, all_args)), leave=False, total=len(all_args)) as pbar:
            for i, x in pbar:
                results.append(x)
                pbar.update()

    return results


def dict_from_list(l):
    d = {}
    for row in l:
        d_inner = d
        for k in row[:-2]:
            if k not in d_inner.keys():
                d_inner[k] = {}
            d_inner = d_inner[k]
        d_inner[row[-2]] = row[-1]
    return d
