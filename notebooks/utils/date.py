import datetime as dt


def resolve_multiple(args, f):
    if len(args) == 0:
        return None
    elif len(args) == 1:
        return f(args[0])
    else:
        return [f(arg) for arg in args]


def timestamp_to_date(*timestamps):
    def f(x):
        return dt.datetime.utcfromtimestamp(int(x))
    return resolve_multiple(timestamps, f)


def str_time_to_datetime(*str_times):
    def f(x):
        return dt.datetime.strptime(x, "%Y-%m-%d %H:%M:%S")
    return resolve_multiple(str_times, f)


def str_time_to_timestamp(*str_times):
    def f(x):
        return str_time_to_datetime(x).timestamp()
    return resolve_multiple(str_times, f)


