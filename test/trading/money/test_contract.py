from .test_money import TestMoney


class TestContract(TestMoney):

    def test___add__(self):
        self.assertEqual(self.czk(10) + self.czk(4), self.czk(14))
        self.assertEqual(self.czk(10) + 4, self.czk(14))
        self.assertEqual(4 + self.czk(10), self.czk(14))

        with self.assertRaises(ValueError):
            self.btc(10) + self.czk(5)

    def test___sub__(self):
        self.assertEqual(self.czk(10) - self.czk(4), self.czk(6))
        self.assertEqual(self.czk(10) - 4, self.czk(6))

        with self.assertRaises(ValueError):
            self.btc(10) - self.czk(5)

    def test___mul__(self):
        self.assertEqual(self.czk(10) * self.czk(4), self.czk(40))
        self.assertEqual(self.czk(10) * 4, self.czk(40))
        self.assertEqual(4 * self.czk(10), self.czk(40))

        with self.assertRaises(ValueError):
            self.btc(10) * self.czk(5)

    def test___div__(self):
        self.assertEqual(self.czk(10) / self.czk(2), self.czk(5))
        self.assertEqual(self.czk(10) / 2, self.czk(5))

        with self.assertRaises(ValueError):
            self.btc(10) / self.czk(5)
