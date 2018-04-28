import sys
paths = ["D:\\GitProjects\\krypl-project", "D:\\GitProjects\\krypl-project\\notebooks"]
for p in paths:
    sys.path.append(p)

from trading.exchange import BackTestExchange
from trading.dataManager import CurrencyDataManager
from trading.money.contract import ContractPair, Contract
from trading.statistics import Statistics
from trading.money.transaction import Transaction

from calculator.talibWrapper import calculateTalib