import numpy as np


def getCorrectType(value):
    try:
        converted = float(value)
        return converted
    except ValueError:
        return value


def paramsFromPayload(payload):
    return {kv['name']: getCorrectType(kv['value']) for kv in payload['args']}


def inputsFromPayload(payload):
    return {k: np.array(v) for k, v in payload['data'].items()}

