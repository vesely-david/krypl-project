from calculator.talibWrapper import allTalibFunctions, functionInfo
from falcon import HTTPError


def availableFunctions():
    jsonData = {
        'TALIB': allTalibFunctions()
    }
    return jsonData


def functionInfo(group, functionName):
    if group == 'TALIB':
        try:
            fInfo = functionInfo(functionName)
            return {functionName: fInfo}
        except:
            message = "Function '{fName}' in group '{group}' does not exist.".format(fname=functionName, group=group)
            raise HTTPError(message, code=501)

    message = "Group '{group}' does not exist.".format(group=group)
    raise HTTPError(message, code=501)


def calculateData():
    raise
