from copy import deepcopy

import matplotlib.pyplot as plt
import numpy as np
import pandas as pd
import seaborn as sns
from IPython.display import display, Markdown, HTML
from sklearn.metrics import precision_score, recall_score, f1_score, confusion_matrix
from utils.common import load_split, load_model
from utils.commonImports import axis_font, title_font

from trading.dataManager import CurrencyDataManager
from trading.exchange import BackTestExchange
from modeling.strategy import MLStrategy, HoldStrategy


def gs_stats(grid_search, X, y):
    precisions = []
    recalls = []
    f1s = []
    for clf in grid_search.clfs:
        y_pred = clf.predict(X)
        precisions.append(precision_score(y, y_pred))
        recalls.append(recall_score(y, y_pred))
        f1s.append(f1_score(y, y_pred))

    stats = pd.DataFrame({
        'precision': precisions,
        'recall': recalls,
        'f1': f1s
    })
    return stats


def precision_recall_curve(precisions, recalls, title):
    plt.figure(figsize=[12, 6])
    sns.regplot(np.array(precisions), np.array(recalls), order=2)
    plt.title(title, fontproperties=title_font)
    plt.xlabel('precision', fontproperties=axis_font)
    plt.ylabel('recall', fontproperties=axis_font)


def display_md(text):
    display(Markdown(text))


def display_pandas(df):
    display(HTML(df.to_html()))


def gs_report(data_root, gs_path):
    X_train, y_train, X_val, y_val, _, _ = load_split(data_root)
    grid_search = load_model(gs_path)

    stats_train = gs_stats(grid_search, X_train, y_train)
    precision_recall_curve(stats_train['precision'], stats_train['recall'], 'Train set - precision recall')

    stats_val = gs_stats(grid_search, X_val, y_val)
    precision_recall_curve(stats_val['precision'], stats_val['recall'], 'Val set - precision recall')
    return grid_search, stats_val


def pandas_df_to_markdown_table(df):
    fmt = ['---' for _ in range(len(df.columns))]
    df_fmt = pd.DataFrame([fmt], columns=df.columns)
    df_formatted = pd.concat([df_fmt, df])
    display_md(df_formatted.to_csv(sep="|"))


def cm(y_true, y_pred):
    conf_matt = confusion_matrix(y_true, y_pred)
    conf_matt = pd.DataFrame(conf_matt, columns=['predicted_0', 'predicted_1'], index=['true_0', 'true_1'])
    return conf_matt


def print_clf_eval(clf, X, y_true):
    y_pred = clf.predict(X)
    conf_matrix = cm(y_true, y_pred)
    display_pandas(conf_matrix)
    print(f'Precision: %.3f' % precision_score(y_true, y_pred))
    print(f'Recall: %.3f' % recall_score(y_true, y_pred))
    print()


def clf_report(clf, data_root):
    X_train, y_train, X_val, y_val, X_test, y_test= load_split(data_root)
    display_md("## Train data")
    print_clf_eval(clf, X_train, y_train)

    display_md("## Validation data")
    print_clf_eval(clf, X_val, y_val)

    display_md("## Test data")
    print_clf_eval(clf, X_test, y_test)


def hold_strategy_stats(data_manager_p, contract_pair, wallet):
    data_manager = deepcopy(data_manager_p)
    exchange = BackTestExchange(data_manager, deepcopy(wallet), 0.0025)
    strategy = HoldStrategy(exchange, data_manager, contract_pair, 10)
    strategy.trade()
    return strategy.stats(contract_pair['priceContract']).report()


def strategy_report(data, price_data, strategy, contract_pair, wallet):
    data_manager = CurrencyDataManager(price_data, data)
    hold_stats = hold_strategy_stats(data_manager, contract_pair, wallet)
    strategy.trade()
    strategy_stats = strategy.stats(contract_pair['priceContract']).report()
    stats = hold_stats.join(strategy_stats, lsuffix='_hold', rsuffix='_strategy')
    display_pandas(stats)
    return stats
