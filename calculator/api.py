from calculator.talibWrapper import allTalibFunctions, talibFunctionInfo, calculateTalib
import calculator.talibWrapper as talibW
from calculator.utils import paramsFromPayload, inputsFromPayload
from enum import Enum


class FunctionGroups(Enum):
    TALIB = 'TALIB'

    @staticmethod
    def toList():
        return list(map(lambda c: c.value, FunctionGroups))


def availableFunctions():
    jsonData = {
        FunctionGroups.TALIB.value: allTalibFunctions()
    }
    return jsonData


def functionInfo(group, functionName):
    group, functionName = group.upper(), functionName.upper()
    if group == FunctionGroups.TALIB.value:
        fInfo = talibFunctionInfo(functionName)
        return {functionName: fInfo}


def calculateData(group, functionName, functionArgs):
    group, functionName = group.upper(), functionName.upper()
    if group == FunctionGroups.TALIB.value:
        inputs = inputsFromPayload(functionArgs)
        params = paramsFromPayload(functionArgs)
        result = calculateTalib(functionName, inputs, params)
        return {
            'result': [r.tolist() for r in result]
        }
    raise Exception('Group {group} is not implemented by API.'.format(group=group))


def isGroupAndNameValid(group, functionName):
    group, functionName = group.upper(), functionName.upper()
    if group not in FunctionGroups.toList():
        return False, "Group {group} is not implemented.".format(**locals())

    if not talibW.isFunctionValid(functionName):
        return False, "Function {functionName} of group {group} is not implemented.".format(**locals())

    return True
