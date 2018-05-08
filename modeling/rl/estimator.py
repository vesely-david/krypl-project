"""
Reference - MLP estimators were highly inspired by project https://github.com/dennybritz/reinforcement-learning and
https://gist.github.com/kkweon/c8d1caabaf7b43317bc8825c226045d2
"""
import numpy as np
import tensorflow as tf
from keras.optimizers import Adam
from keras import layers
from keras.models import Model
from keras import backend as K
from keras import utils as np_utils


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


class MLPPolicyEstimator(PolicyEstimator):

    def __init__(self, input_dim, output_dim, hidden_dims=[32, 32]):
        self.input_dim = input_dim
        self.output_dim = output_dim
        self.__build_network(input_dim, output_dim, hidden_dims)
        self.__build_train_fn()

    def __build_network(self, input_dim, output_dim, hidden_dims=[32, 32]):
        self.X = layers.Input(shape=(input_dim,))
        net = self.X

        for h_dim in hidden_dims:
            net = layers.Dense(h_dim)(net)
            net = layers.Activation("relu")(net)

        net = layers.Dense(output_dim)(net)
        net = layers.Activation("softmax")(net)

        self.model = Model(inputs=self.X, outputs=net)

    def __build_train_fn(self):
        action_prob_placeholder = self.model.output
        action_onehot_placeholder = K.placeholder(shape=(None, self.output_dim),
                                                  name="action_onehot")
        discount_reward_placeholder = K.placeholder(shape=(None,),
                                                    name="discount_reward")

        action_prob = K.sum(action_prob_placeholder * action_onehot_placeholder, axis=1)
        log_action_prob = K.log(action_prob)

        loss = - log_action_prob * discount_reward_placeholder
        loss = K.mean(loss)
        adam = Adam()
        updates = adam.get_updates(params=self.model.trainable_weights,
                                   loss=loss)

        self.train_fn = K.function(inputs=[self.model.input,
                                           action_onehot_placeholder,
                                           discount_reward_placeholder],
                                   outputs=[],
                                   updates=updates)

    def predict(self, state):
        action_prob = self.model.predict(state)
        return action_prob[0]

    def update(self, state, target, action):
        action_onehot = np_utils.to_categorical(action, num_classes=self.output_dim)
        discount_reward = np.array([target])
        self.train_fn([state, action_onehot, discount_reward])


class PerceptronValueEstimator(ValueEstimator):
    def __init__(self, numOfFeatures, learningRate=0.1):
        with tf.variable_scope("perceptron_value_estimator"):
            self.state = tf.placeholder(tf.float32, [None, numOfFeatures], "state")
            self.target = tf.placeholder(dtype=tf.float32, name="target")

            self.outputLayer = tf.contrib.layers.fully_connected(
                inputs=self.state,
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
