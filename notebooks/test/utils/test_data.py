from unittest import TestCase
from utils.data import ModelingData
import pandas as pd


class TestData(TestCase):

    def setUp(self):
        df = pd.DataFrame({
            'timestamp': list(range(102)),
            'date': [f'{y}-01-01' for y in range(2000, 2102)],
            'label': [0] * 102,
            'f1': [10] * 102,
            'f2': [20] * 102
        })

        self.modeling_data = ModelingData(df, ['timestamp', 'date', 'label'], '2001-01-01', '2101-01-01')

    def test_X_has_only_feature_cols(self):
        self.assertListEqual(list(self.modeling_data.X_train.columns), ['f1', 'f2'])
        self.assertListEqual(list(self.modeling_data.X_val.columns), ['f1', 'f2'])
        self.assertListEqual(list(self.modeling_data.X_test.columns), ['f1', 'f2'])

    def test_train_sizes(self):
        X_size = len(self.modeling_data.X_train)
        y_size = len(self.modeling_data.y_train)
        self.assertEqual(X_size, 49)
        self.assertEqual(y_size, 49)

    def test_validation_sizes(self):
        X_size = len(self.modeling_data.X_val)
        y_size = len(self.modeling_data.y_val)
        self.assertEqual(X_size, 21)
        self.assertEqual(y_size, 21)

    def test_test_size(self):
        X_size = len(self.modeling_data.X_test)
        y_size = len(self.modeling_data.y_test)
        self.assertEqual(X_size, 30)
        self.assertEqual(y_size, 30)

    def test_strategy_testing_data(self):
        self.assertEqual(self.modeling_data.strategy_testing_data['date'].min(), '2101-01-01')
