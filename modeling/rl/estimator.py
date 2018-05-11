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
            hidden = tf.nn.dropout(hidden, 0.5)
        return hidden


class MLPPolicyEstimator(PolicyEstimator, TensorflowEstimator):

    def __init__(self, input_dim, output_dim, hidden_dims, learning_rate=0.01):
        with tf.variable_scope("perceptron_policy_estimator"):
            self.state = tf.placeholder(tf.float32, [None, input_dim], "state")
            self.action = tf.placeholder(dtype=tf.int32, name="action")
            self.target = tf.placeholder(dtype=tf.float32, name="target")

            hidden = self.add_hidden(self.state, hidden_dims)

            self.output_layer = tf.contrib.layers.fully_connected(
                inputs=hidden,
                num_outputs=output_dim,
                activation_fn=None,
                weights_initializer=tf.zeros_initializer)

            self.action_probs = tf.squeeze(tf.nn.softmax(self.output_layer))
            self.picked_action_prob = tf.gather(self.action_probs, self.action)
            self.loss = -tf.log(self.picked_action_prob) * self.target
            self.optimizer = tf.train.AdamOptimizer(learning_rate=learning_rate)
            self.train_op = self.optimizer.minimize(self.loss, global_step=tf.train.get_global_step())

    def predict(self, state):
        sess = tf.get_default_session()
        return sess.run(self.action_probs, {self.state: state})

    def update(self, state, target, action):
        sess = tf.get_default_session()
        feed_dict = {self.state: state, self.target: target, self.action: action}
        _, loss = sess.run([self.train_op, self.loss], feed_dict)
        return loss


class MLPValueEstimator(ValueEstimator, TensorflowEstimator):
    def __init__(self, input_dim, hidden_dims=[], learning_rate=0.1):
        with tf.variable_scope("perceptron_value_estimator"):
            self.state = tf.placeholder(tf.float32, [None, input_dim], "state")
            self.target = tf.placeholder(dtype=tf.float32, name="target")

            hidden = self.add_hidden(self.state, hidden_dims)

            self.output_layer = tf.contrib.layers.fully_connected(
                inputs=hidden,
                num_outputs=1,
                activation_fn=None,
                weights_initializer=tf.zeros_initializer
            )

            self.value_estimate = tf.squeeze(self.output_layer)
            self.loss = tf.squared_difference(self.value_estimate, self.target)
            self.optimizer = tf.train.AdamOptimizer(learning_rate=learning_rate)
            self.trainOp = self.optimizer.minimize(self.loss, global_step=tf.train.get_global_step())

    def predict(self, state):
        sess = tf.get_default_session()
        return sess.run(self.value_estimate, {self.state: state})

    def update(self, state, target):
        sess = tf.get_default_session()
        feed_dict = {self.state: state, self.target: target}
        _, loss = sess.run([self.trainOp, self.loss], feed_dict)
        return loss
