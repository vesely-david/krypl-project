from calculator.talibWrapper import allTalibFunctions, talibFunctionInfo
import calculator.talibWrapper as talibW
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
    if group == FunctionGroups.TALIB.value:
        fInfo = talibFunctionInfo(functionName)
        return {functionName: fInfo}


def calculateData():
    raise


def isGroupAndNameValid(group, functionName):
    if group not in FunctionGroups.toList():
        return False, "Group {group} is not implemented.".format(**locals())

    if not talibW.isFunctionValid(functionName):
        return False, "Function {functionName} of group {group} is not implemented.".format(**locals())

    return True
