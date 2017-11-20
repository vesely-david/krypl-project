from talib import abstract
from talib import get_functions


def allTalibFunctions():
    return get_functions()


def functionInfo(functionName):
    info = abstract.Function(functionName)
    return str(info)
