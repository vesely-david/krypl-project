import numpy as np


def random_choice(arr):
    return np.random.choice(np.arange(len(arr)), p=arr)
