class FeatureCreator:
    # define in subclasses
    n = None

    def fromState(self, state):
        raise NotImplementedError()
