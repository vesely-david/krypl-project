class Contract:

    @staticmethod
    def new(name, value):
        return [name, value]

    @classmethod
    def add(cls, contract, value):
        contract[1] += value
        return contract

    @classmethod
    def sub(cls, contract, value):
        contract[1] -= value
        return contract

    @classmethod
    def mul(cls, contract, value):
        contract[1] *= value
        return contract


class ContractPair:
    @staticmethod
    def new(priceContract, tradeContract):
        return {'priceContract': priceContract, 'tradeContract': tradeContract}
