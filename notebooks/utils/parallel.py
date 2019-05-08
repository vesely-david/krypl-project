from multiprocessing import Pool
from tqdm import *


def run_parallel(f, args, n_process=10):
    results = []
    with Pool(processes=n_process) as p:
        if type(args) == list:
            all_args = args
        else:
            all_args = list(args)
        with tqdm_notebook(enumerate(p.imap_unordered(f, all_args)), leave=False, total=len(all_args)) as pbar:
            for i, x in pbar:
                results.append(x)
                pbar.update()

    return results
