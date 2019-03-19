import {
  GET_STRATEGY_OVERVIEW,
  GET_STRATEGY_TRADES,
  GET_STRATEGY_VALUE_HISTORY,
} from './types';

import { strategyService } from '../services';

function getStrategyData(strategyId){
  return dispatch => {
    dispatch(getStrategyOverview(strategyId));
    dispatch(getStrategyTrades(strategyId));
    dispatch(getStrategyHistory(strategyId));
  }
}

function getStrategyOverview(strategyId){
  return dispatch => dispatch({
    type: GET_STRATEGY_OVERVIEW,
    payload: strategyService.getStrategyOverview(strategyId)
  }).catch(err => {
    // dispatch(alertActions.error(err));
  })
}

function getStrategyTrades(strategyId){
  return dispatch => dispatch({
    type: GET_STRATEGY_TRADES,
    payload: async () => {
      const result = await strategyService.getStrategyTrades(strategyId)
      return{
        id: strategyId,
        trades: result,
      }
    }
  }).catch(err => {
    // dispatch(alertActions.error(err));
  })
}

function getStrategyHistory(strategyId){
  return dispatch => dispatch({
    type: GET_STRATEGY_VALUE_HISTORY,
    payload: async () => {
      const result = await strategyService.getStrategyHistory(strategyId)
      return{
        id: strategyId,
        history: result,
      }
    }
  }).catch(err => {
    // dispatch(alertActions.error(err));
  })
}

export const strategyActions = {
  getStrategyData,
}