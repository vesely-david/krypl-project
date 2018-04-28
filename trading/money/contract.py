from ..utils import isNumber, equals, objectHash
from operator import add, sub, mul, truediv


class Contract:

    @staticmethod
    def new(name, value):
        return name, value

    def newValue(self, value):
        return Contract(self.name, value)

    @staticmethod
    def sameContractName(first, second):
        return first[0] == second[0]

    @staticmethod
    def operation(self, other, func):
        if isNumber(other):
            return self.newValue(func(self.value, other))
        elif isinstance(other, Contract):
            if self.sameContractName(other):
                return self.newValue(func(self.value, other.value))
            else:
                raise ValueError("Not same contract names")

        return NotImplemented

    def __mul__(self, other):
        return self.operation(other, mul)

    def __rmul__(self, other):
        return self.operation(other, mul)

    def __add__(self, other):
        return self.operation(other, add)

    def __radd__(self, other):
        return self.operation(other, add)

    def __truediv__(self, other):
        return self.operation(other, truediv)

    def __sub__(self, other):
        return self.operation(other, sub)

    def __eq__(self, o: object) -> bool:
        if equals(self, o):
            return True
        else:
            return super().__eq__(o)

    def __ne__(self, o: object):
        return self != o

    def __hash__(self) -> int:
        return objectHash(self)


class ContractPair:
    @staticmethod
    def new(priceContract, tradeContract):
        return {'priceContract': priceContract, 'tradeContract': tradeContract}
