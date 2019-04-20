import pickle
from sqlite3 import connect

import numpy as np
import pandas as pd
from sklearn.externals import joblib


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
    data = read_table(table, db)
    data = reset_index_hard(data)
    if from_date is not None:
        data = data.query(f"date >= '{from_date}'")
    if to_date is not None:
        data = data.query(f"date < '{to_date}'")
    if period is not None:
        data = data.query(f"period == '{period}'")

    return reset_index_hard(data.sort_values('timestamp'))


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


def save_model(model, file):
    joblib.dump(model, file)


def load_model(file):
    return joblib.load(file)


def fillna(df, what, na_list=[np.inf, -np.inf, np.nan]):
    df_filled = df.copy()
    for na_value in na_list:
        df_filled = df_filled.replace(na_value, what)
    return df_filled.fillna(what)


def dropna(df):
    return fillna(df, np.nan).dropna()


def reset_index_hard(df):
    return df.reset_index().drop('index', axis=1)
