from unittest import TestCase
from trading.money.contract import Contract, ContractPair


class TestMoney(TestCase):
    @staticmethod
    def czk(value):
        return Contract.new('czk', value)

    @staticmethod
    def btc(value):
        return Contract.new('btc', value)

    @staticmethod
    def czk_btc():
        return ContractPair.new('czk', 'btc')
