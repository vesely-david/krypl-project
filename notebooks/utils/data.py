from sklearn.model_selection import train_test_split
from utils.common import reset_index_hard, write_tsv


class ModelingData:
    def __init__(self, data, columns_to_drop, modeling_start, strategy_testing_start):
        self.data = data
        self.columns_to_drop = columns_to_drop
        self.modeling_start = modeling_start
        self.strategy_testing_start = strategy_testing_start
        self.strategy_testing_data = None
        self.modeling_data = None
        self.X_train, self.X_val, self.X_test = None, None, None
        self.y_train, self.y_val, self.y_test = None, None, None

        self.split_trading_data()

    @staticmethod
    def train_validation_test_split(X, y):
        X_trainval, X_test, y_trainval, y_test = train_test_split(X, y, test_size=0.3)
        X_train, X_val, y_train, y_val = train_test_split(X_trainval, y_trainval, test_size=0.3)
        return X_train, X_val, X_test, y_train, y_val, y_test

    @staticmethod
    def drop_columns(df, columns):
        res = df.copy()
        for col in columns:
            res = res.drop(col, axis=1)
        return res

    def filter_data(self, query):
        res = self.data \
            .query(query) \
            .copy() \
            .sort_values('timestamp')
        return reset_index_hard(res)

    def split_trading_data(self):
        self.strategy_testing_data = self.filter_data(f'date >= "{self.strategy_testing_start}"')
        self.modeling_data = self.filter_data(f'"{self.modeling_start}" <= date < "{self.strategy_testing_start}"')

        X = self.drop_columns(self.modeling_data, self.columns_to_drop)
        y = self.modeling_data['label'].apply(int)
        self.X_train, self.X_val, self.X_test, self.y_train, self.y_val, self.y_test = \
            self.train_validation_test_split(X, y)

    def write_split(self, root):
        write_tsv(self.X_train, f"{root}/X_train.tsv")
        write_tsv(self.y_train, f"{root}/y_train.tsv")
        write_tsv(self.X_val, f"{root}/X_val.tsv")
        write_tsv(self.y_val, f"{root}/y_val.tsv")
        write_tsv(self.X_test, f"{root}/X_test.tsv")
        write_tsv(self.y_test, f"{root}/y_test.tsv")

        write_tsv(self.strategy_testing_data, f'{root}/strategy_test.tsv')
        write_tsv(self.modeling_data, f'{root}/strategy_train.tsv')