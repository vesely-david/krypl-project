from .test_money import TestMoney
from trading.money.transaction import Transaction


class TestTransaction(TestMoney):

    def setUp(self):
        self.contract_pair = self.czkBtcContract()
        self.timestamp = 0
        self.transaction_params = {'contractPair': self.contract_pair, 'timestamp': self.timestamp, 'amount': 10.,
                                   'price': 2., 'fee': .5}


class TestSellTransaction(TestTransaction):
    def setUp(self):
        super(TestSellTransaction, self).setUp()
        self.transaction = Transaction.new_sell(**self.transaction_params)

    def test_gainedContract(self):
        expected = self.czk(20.)
        self.assertEqual(Transaction.gained_contract(self.transaction), expected)

    def test_subtractedContract(self):
        expected = self.btc(10.)
        self.assertEqual(Transaction.subtracted_contract(self.transaction), expected)


class TestBuyTransaction(TestTransaction):
    def setUp(self):
        super(TestBuyTransaction, self).setUp()
        self.transaction = Transaction.new_buy(**self.transaction_params)

    def test_gainedContract(self):
        expected = self.btc(10.)
        self.assertEqual(Transaction.gained_contract(self.transaction), expected)

    def test_subtractedContract(self):
        expected = self.czk(20.)
        self.assertEqual(Transaction.subtracted_contract(self.transaction), expected)
