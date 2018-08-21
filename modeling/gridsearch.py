import itertools
import time


class GridSearch:
    def __init__(self, classifier_func, params):
        self.classifier_func = classifier_func
        self.params = params
        self.clfs = []

    def params_list(self):
        values = self.params.values()
        keys = list(self.params.keys())
        combinations = list(itertools.product(*values))
        return [{keys[i]: c[i] for i in range(len(keys))} for c in combinations]

    def fit_all(self, X, y):
        params_list_ = self.params_list()
        for i, params in enumerate(params_list_):
            start_time = time.time()
            print(f'train [{i}/{len(params_list_)}] {params}', end='')
            clf = self.classifier_func(**params)
            clf.fit(X, y)
            self.clfs.append(clf)
            print('........................... %.2f sec' % (time.time() - start_time))