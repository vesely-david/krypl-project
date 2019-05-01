from numbers import Number


def isNumber(x):
    return isinstance(x, Number)


def equals(x, y):
    if type(x) == type(y):
        return objectKey(x) == objectKey(y)

    return False


def objectKey(x):
    return tuple(dir(x))


def objectHash(x):
    return hash(objectKey(x))


def combine_dicts(d1, d2):
    keys = list(d1.keys()) + list(d2.keys())
    d = {}
    for k in keys:
        v1, v2 = d1.get(k, None), d2.get(k, None)
        if v1 is None:
            d[k] = v2
        elif v2 is None:
            d[k] = v1
        elif type(v1) == dict:
            d[k] = combine_dicts(v1, v2)
        else:
            d[k] = v1 + v2
    return d
