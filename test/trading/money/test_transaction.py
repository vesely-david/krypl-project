from .test_money import TestMoney
from trading.money.transaction import SellTransaction, BuyTransaction, Transaction


class TestTransaction(TestMoney):

    def setUp(self):
        self.contractPair = self.czkBtcContract()
        self.timestamp = 0
        self.transactionParams = {'contractPair': self.contractPair, 'timestamp': self.timestamp, 'amount': 10.,
                                  'price': 2.,
                                  'fee': .5}

    def assertTransaction(self, actual, expectedDict):
        self.assertEqual(actual.contractPair, expectedDict['contractPair'])
        self.assertEqual(actual.timestamp, expectedDict['timestamp'])
        self.assertEqual(actual.amount, expectedDict['amount'])
        self.assertEqual(actual.price, expectedDict['price'])

    def test_init(self):
        transaction = Transaction(**self.transactionParams)
        expectedDict = {'contractPair': self.contractPair, 'timestamp': self.timestamp, 'amount': self.btc(10.),
                        'price': self.czk(2.), 'fee': self.czk(.5)}
        self.assertTransaction(transaction, expectedDict)


class TestSellTransaction(TestTransaction):
    def setUp(self):
        super(TestSellTransaction, self).setUp()
        self.transaction = SellTransaction(**self.transactionParams)

    def test_gainedContract(self):
        expected = self.czk(20.)
        self.assertEqual(self.transaction.gainedContract(), expected)

    def test_subtractedContract(self):
        expected = self.btc(10.)
        self.assertEqual(self.transaction.subtractedContract(), expected)


class TestBuyTransaction(TestTransaction):
    def setUp(self):
        super(TestBuyTransaction, self).setUp()
        self.transaction = BuyTransaction(**self.transactionParams)

    def test_gainedContract(self):
        expected = self.btc(10.)
        self.assertEqual(self.transaction.gainedContract(), expected)

    def test_subtractedContract(self):
        expected = self.czk(20.)
        self.assertEqual(self.transaction.subtractedContract(), expected)
