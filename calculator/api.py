from calculator.talibWrapper import allTalibFunctions
import json


def availableFunctions():
    talibFuncNames = ['TALIB-%s' % name for name in allTalibFunctions()]
    jsonData = json.dumps({
        'names': talibFuncNames
    })
    return jsonData


def functionInfo():
    raise


def calculateData():
    raise
