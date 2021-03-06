from talib import abstract
from talib import get_functions


def allTalibFunctions():
    return get_functions()


def isFunctionValid(functionName):
    return functionName in allTalibFunctions()


def talibFunctionInfo(functionName):
    info = abstract.Function(functionName)
    return str(info)


def calculateTalib(functionName, inputs, params):
    f = abstract.Function(functionName)
    f.set_input_arrays(inputs)
    f.set_parameters(params)
    result = f.run()
    return result
