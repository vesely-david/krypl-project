"""
Reference - MLP estimators were highly inspired by project https://github.com/dennybritz/reinforcement-learning and
https://gist.github.com/kkweon/c8d1caabaf7b43317bc8825c226045d2
"""
import tensorflow as tf


class PolicyEstimator:
    def predict(self, state):
        raise NotImplementedError()

    def update(self, state, target, action):
        raise NotImplementedError()


class ValueEstimator:
    def predict(self, state):
        raise NotImplementedError()

    def update(self, state, target):
        raise NotImplementedError()


class TensorflowEstimator:

    @staticmethod
    def add_hidden(_input, hidden_dims):
        hidden = _input
        for dim in hidden_dims:
            hidden = tf.contrib.layers.fully_connected(
                inputs=hidden,
                num_outputs=dim,
                activation_fn=tf.nn.relu,
                weights_initializer=tf.zeros_initializer
            )
        return hidden


class MLPPolicyEstimator(PolicyEstimator, TensorflowEstimator):

    def __init__(self, numOfFeatures, numOfOutputs, hidden_dims, learningRate=0.01):
        with tf.variable_scope("perceptron_policy_estimator"):
            self.state = tf.placeholder(tf.float32, [None, numOfFeatures], "state")
            self.action = tf.placeholder(dtype=tf.int32, name="action")
            self.target = tf.placeholder(dtype=tf.float32, name="target")

            hidden = self.add_hidden(self.state, hidden_dims)

            self.outputLayer = tf.contrib.layers.fully_connected(
                inputs=hidden,
                num_outputs=numOfOutputs,
                activation_fn=None,
                weights_initializer=tf.zeros_initializer)

            self.actionProbs = tf.squeeze(tf.nn.softmax(self.outputLayer))
            self.pickedActionProb = tf.gather(self.actionProbs, self.action)
            self.loss = -tf.log(self.pickedActionProb) * self.target
            self.optimizer = tf.train.AdamOptimizer(learning_rate=learningRate)
            self.trainOp = self.optimizer.minimize(self.loss, global_step=tf.train.get_global_step())

    def predict(self, state):
        sess = tf.get_default_session()
        return sess.run(self.actionProbs, {self.state: state})

    def update(self, state, target, action):
        sess = tf.get_default_session()
        feedDict = {self.state: state, self.target: target, self.action: action}
        _, loss = sess.run([self.trainOp, self.loss], feedDict)
        return loss


class MLPValueEstimator(ValueEstimator, TensorflowEstimator):
    def __init__(self, input_dim, hidden_dims=[], learningRate=0.1):
        with tf.variable_scope("perceptron_value_estimator"):
            self.state = tf.placeholder(tf.float32, [None, input_dim], "state")
            self.target = tf.placeholder(dtype=tf.float32, name="target")

            hidden = self.add_hidden(self.state, hidden_dims)

            self.outputLayer = tf.contrib.layers.fully_connected(
                inputs=hidden,
                num_outputs=1,
                activation_fn=None,
                weights_initializer=tf.zeros_initializer
            )

            self.valueEstimate = tf.squeeze(self.outputLayer)
            self.loss = tf.squared_difference(self.valueEstimate, self.target)
            self.optimizer = tf.train.AdamOptimizer(learning_rate=learningRate)
            self.trainOp = self.optimizer.minimize(self.loss, global_step=tf.train.get_global_step())

    def predict(self, state):
        sess = tf.get_default_session()
        return sess.run(self.valueEstimate, {self.state: state})

    def update(self, state, target):
        sess = tf.get_default_session()
        feedDict = {self.state: state, self.target: target}
        _, loss = sess.run([self.trainOp, self.loss], feedDict)
        return loss
