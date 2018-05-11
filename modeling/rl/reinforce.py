import itertools
import numpy as np
from collections import namedtuple
import sys
from modeling.rl.utils import random_choice


EpisodeStats = namedtuple("Stats", ["episode_rewards"])
Transition = namedtuple("Transition", ["state", "action", "reward", "next_state", "done", "debug"])


def proceed_episode(i_episode, env, estimator_policy, stats, num_episodes, get_state):
    state = get_state(env.reset())
    episode = []

    for t in itertools.count():
        action_probs = estimator_policy.predict(state)
        action = random_choice(action_probs)
        next_state, reward, done, debug = env.step(action)
        next_state = get_state(next_state)

        episode.append(
            Transition(state=state, action=action, reward=reward, next_state=next_state, done=done, debug=debug))
        stats.episode_rewards[i_episode] += reward
        sys.stdout.write("\rStep {} @ Episode {}/{} ({})".format(t, i_episode + 1, num_episodes,
                                                      stats.episode_rewards[i_episode - 1]))
        sys.stdout.flush()
        if done:
            break
        state = next_state

    return episode


def update_estimators(episode, estimator_policy, estimator_value, discount_factor):
    for t, transition in enumerate(episode):
        # The return after this timestep
        total_return = sum(discount_factor ** i * t.reward for i, t in enumerate(episode[t:]))
        # Calculate baseline/advantage
        baseline_value = estimator_value.predict(transition.state)
        advantage = total_return - baseline_value
        # Update our value estimator
        estimator_value.update(transition.state, total_return)
        # Update our policy estimator
        estimator_policy.update(transition.state, advantage, transition.action)


def reinforce(env, estimator_policy, estimator_value, num_episodes, get_state=lambda x: x, discount_factor=1.0):
    """
    REINFORCE (Monte Carlo Policy Gradient) Algorithm. Optimizes the policy
    function approximator using policy gradient. Ref: https://github.com/dennybritz/reinforcement-learning

    Args:
        env: OpenAI environment.
        estimator_policy: Policy Function to be optimized
        estimator_value: Value function approximator, used as a baseline
        num_episodes: Number of episodes to run for
        get_state: Function which which converts environment state to feature vector for models.
        discount_factor: Time-discount factor

    Returns:
        An EpisodeStats object with two numpy arrays for episode_lengths and episode_rewards.
    """

    stats = EpisodeStats(episode_rewards=np.zeros(num_episodes))
    episodes = []
    for i_episode in range(num_episodes):
        episode = proceed_episode(i_episode, env, estimator_policy, stats, num_episodes, get_state)
        episodes.append(episode)
        update_estimators(episode, estimator_policy, estimator_value, discount_factor)

    return stats, episodes
