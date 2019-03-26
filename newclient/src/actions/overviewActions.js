import {
  GET_PAPER_OVERVIEW,
  GET_PAPER_STRATEGIES,
  REGISTER_PAPER_STRATEGY,
  GET_REAL_OVERVIEW,
  GET_REAL_STRATEGIES,
  REGISTER_REAL_STRATEGY,
} from './types';

import { overviewService } from '../services/overviewService';
import { assetActions } from './assetActions';

function getPaperOverview(){
  return dispatch => dispatch({
    type: GET_PAPER_OVERVIEW,
    payload: overviewService.getPaperOverview()
  }).catch(err => {
    // dispatch(alertActions.error(err));
  })
}

function getPaperStrategies(){
  return dispatch => dispatch({
    type: GET_PAPER_STRATEGIES,
    payload: async () =>{
      const res = await overviewService.getPaperStrategies();
      return{
        strategies: res.strategies,
        pagination:{
          page: res.page,
          perPage: res.perPage,
          total: res.count,
        },
      }
    }
  }).catch(err => {
    // dispatch(alertActions.error(err));
  })
}

function registerPaperStrategy(name, exchange, description, assets){
  return dispatch => dispatch({
    type: REGISTER_PAPER_STRATEGY,
    payload: async () => {
      const res = await overviewService.registerPaperStrategy(name, exchange, description, assets);
      dispatch(getPaperOverview());
      dispatch(getPaperStrategies());
      dispatch(assetActions.getAssets());
      return res;
    }
  }).catch(err => {
    // dispatch(alertActions.error(err));
  })
}


function getRealOverview(){
  return dispatch => dispatch({
    type: GET_REAL_OVERVIEW,
    payload: overviewService.getRealOverview()
  }).catch(err => {
    // dispatch(alertActions.error(err));
  })
}

function getRealStrategies(){
  return dispatch => dispatch({
    type: GET_REAL_STRATEGIES,
    payload: async () =>{
      const res = await overviewService.getRealStrategies();
      return{
        strategies: res.strategies,
        pagination:{
          page: res.page,
          perPage: res.perPage,
          total: res.count,
        },
      }
    }
  }).catch(err => {
    // dispatch(alertActions.error(err));
  })
}

function registerRealStrategy(name, exchange, description, assets){
  return dispatch => dispatch({
    type: REGISTER_REAL_STRATEGY,
    payload: async () => {
      const res = await overviewService.registerRealStrategy(name, exchange, description, assets);
      dispatch(getRealOverview());
      dispatch(getRealStrategies());
      dispatch(assetActions.getAssets());
      return res;
    }
  }).catch(err => {
    // dispatch(alertActions.error(err));
  })
}

export const overviewActions = {
  getPaperOverview,
  getPaperStrategies,
  registerPaperStrategy,
  getRealOverview,
  getRealStrategies,
  registerRealStrategy,
}
