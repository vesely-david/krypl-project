import {
  GET_PAPER_STRATEGY,
  GET_STRATEGY_TRADES,
  GET_STRATEGY_VALUE_HISTORY,
  STOP_STRATEGY,
} from './types';
import { strategyService } from '../services';
import { assetActions } from '../actions/assetActions';


const actionTranslator = {
  'papertesting': GET_PAPER_STRATEGY,
}

function getStrategyData(strategyId){
  return dispatch => {
    dispatch(getStrategyTrades(strategyId));
    dispatch(getStrategyHistory(strategyId));
  }
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

function getStrategy(strategyId, tradingMode){
  return dispatch => dispatch({
    type: actionTranslator[tradingMode.toLowerCase()],
    payload: strategyService.getStrategy(strategyId, tradingMode)
  }).catch(err => {
    // dispatch(alertActions.error(err));
  })
}

function stopStrategy(strategyId, tradingMode){
  return dispatch => dispatch({
    type: STOP_STRATEGY,
    payload: async () => {
      await strategyService.stopStrategy(strategyId)
      dispatch(getStrategy(strategyId, tradingMode));
      dispatch(assetActions.getAssets());
      return true;
    }
  }).catch(err => {
    // dispatch(alertActions.error(err));
    return false;
  })
}

export const strategyActions = {
  getStrategyData,
  getStrategy,
  stopStrategy,
}