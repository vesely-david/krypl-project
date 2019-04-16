import numpy as np


def supres(ltp, n):
    """
    This function takes a numpy array of last traded price
    and returns a list of support and resistance levels
    respectively. n is the number of entries to be scanned.
    """
    from scipy.signal import savgol_filter as smooth

    # converting n to a nearest even number
    if n % 2 != 0:
        n += 1
    n_half = int(n / 2)

    n_ltp = ltp.shape[0]

    # smoothening the curve
    ltp_s = smooth(ltp, n + 1, 3)
    ltp_s = np.insert(ltp_s[n + 1:], 0, [0] * (n + 1))

    # taking a simple derivative
    ltp_d = np.zeros(n_ltp)
    ltp_d[1:] = np.subtract(ltp_s[1:], ltp_s[:-1])

    resistance = []
    support = []

    for i in range(n_ltp - n):
        arr_sl = ltp_d[i:(i + n)]
        first = arr_sl[:n_half]  # first half
        last = arr_sl[n_half:]  # second half

        r_1 = np.sum(first > 0)
        r_2 = np.sum(last < 0)

        s_1 = np.sum(first < 0)
        s_2 = np.sum(last > 0)

        # local maxima detection
        if (r_1 == n_half) and (r_2 == n_half):
            k = i + (n_half - 1)
            resistance.append([k, ltp[k]])

        # local minima detection
        if (s_1 == n_half) and (s_2 == n_half):
            k = i + (n_half - 1)
            support.append([k, ltp[k]])

    return np.array(support), np.array(resistance)
