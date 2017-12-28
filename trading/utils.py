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
