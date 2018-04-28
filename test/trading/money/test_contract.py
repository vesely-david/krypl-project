from .test_money import TestMoney
from trading.money.contract import Contract


class TestContract(TestMoney):

    def test_add(self):
        self.assertEqual(Contract.add(self.czk(10), 4), self.czk(14))

    def test_sub(self):
        self.assertEqual(Contract.sub(self.czk(10), 4), self.czk(6))

    def test_mul(self):
        self.assertEqual(Contract.mul(self.czk(10), 4), self.czk(40))
