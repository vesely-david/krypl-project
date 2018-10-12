from unittest import TestCase
from utils.data import ModelingData


class TestData(TestCase):

    def test_split(self):
        data = pd.DataFrame({
            'timestamp': list(range(100))
        })