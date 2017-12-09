from unittest import TestCase
from trading.money.contract import Contract, ContractPair


class TestMoney(TestCase):
    @staticmethod
    def czk(value):
        return Contract('czk', value)

    @staticmethod
    def btc(value):
        return Contract('btc', value)

    @staticmethod
    def czkBtcContract():
        return ContractPair('czk', 'btc')
