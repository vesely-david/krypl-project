class Contract:
    def __init__(self, name, value):
        self.name = name
        self.value = value


class PairBalance:
    def __init__(self, namePrimary, valuePrimary, nameSecondary, valueSecondary=0):
        self.primaryContract = Contract(namePrimary, valuePrimary)
        self.secondaryContract = Contract(nameSecondary, valueSecondary)
